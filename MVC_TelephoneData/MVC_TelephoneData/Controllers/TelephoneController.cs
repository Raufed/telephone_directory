using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Npgsql;
using System.Data;
using MVC_TelephoneData.Models;
using System.Threading.Tasks;

namespace MVC_TelephoneData.Controllers
{
    public class TelephoneController : Controller
    {
        
        // GET: Telephone

        public ActionResult Index(string SName, string FName, string patron,string Poststring = "", string Organstring ="", string Sort = "0",  int Page = 1,int Limit = 9)
        {
            ViewData["Organstring"] = Organstring;
            ViewData["Poststring"] = Poststring;

            ViewData["Sort"] = Sort;
            ViewData["SName"] = SName;
            ViewData["FName"] = FName;
            ViewData["patron"] = patron;

            ViewData["Limit"] = Limit;
            ViewData["Page"] = Page;

            string Offset = $"OFFSET {Limit * (Page - 1)}";
            string LimitSuka = $"LIMIT {Limit}";

            string sort = Sort == "0" ? "ASC" : "DESC";

            int countPosit = 1;

            List<TelephoneModel> displaytelephone = new List<TelephoneModel>();
            NpgsqlConnection conn = new NpgsqlConnection("Server=localhost;Port=5432;Database=Telephons;User Id=postgres;Password=post2001gre2003sql;");
            conn.Open();
            NpgsqlCommand comm = new NpgsqlCommand();

            comm.Connection = conn;
            comm.CommandType = CommandType.Text;
            comm.CommandText = $@"
                SELECT 
                    firstname,
                    secondname,
                    patronymic,
                    organ.orgname,
                   
                    postname,
                    code,
                    telephone.telephone,
                    longtelephone 
                FROM WORKER 
                INNER JOIN organ 
                ON (fk_organ_worker = organ.idorgan) 
                INNER JOIN telephone 
                ON(idworker = fk_telephone_worker) 
                INNER JOIN post 
                ON(idpost = fk_post_worker) 
                INNER JOIN telcode ON( idtelcode = idorgan)
                WHERE 
                (orgname LIKE '%{Organstring}%' AND postname LIKE '%{Poststring}%')
                ORDER BY secondname {sort} {LimitSuka} {Offset}";
            //comm.CommandText = "select * from worker";

            NpgsqlDataReader sdr = comm.ExecuteReader();
            while(sdr.Read())
            {
                var tellist = new TelephoneModel();
                countPosit++;
                //tellist.idworker = Convert.ToInt32(sdr["idworker"]);
                tellist.firstname = sdr["firstname"].ToString();
                tellist.secondname = sdr["secondname"].ToString();
                tellist.patronymic = sdr["patronymic"].ToString();
                tellist.orgname = sdr["orgname"].ToString();
                tellist.postname = sdr["postname"].ToString();
                tellist.code = sdr["code"].ToString();
                tellist.telephone = (sdr["telephone"]).ToString();
                tellist.longtelephone = (sdr["longtelephone"]).ToString();

                if (SName == "")
                {
                    displaytelephone.Add(tellist);
                    continue;
                }
                else if (SName != tellist.secondname && SName != null)
                {
                     continue;
                }
                else if(SName == tellist.secondname)
                {
                    displaytelephone.Add(tellist);
                }
                else if (SName == tellist.secondname && FName == tellist.firstname)
                {
                    displaytelephone.Add(tellist);
                }
                else if (SName == tellist.secondname && FName == tellist.firstname && patron == tellist.patronymic)
                {
                    displaytelephone.Add(tellist);
                }
                else 
                displaytelephone.Add(tellist);
            }

            sdr.Close();

            comm.CommandText = "SELECT COUNT(*) AS count FROM WORKER";
            sdr = comm.ExecuteReader();
            sdr.Read();
            int WorkerCount = Convert.ToInt32(sdr["count"]);
            ViewData["PageCount"] = Math.Ceiling((decimal)WorkerCount / (decimal)Limit);

            sdr.Close();

            comm.CommandText = "SELECT orgname FROM organ";
            sdr = comm.ExecuteReader();
            List<string> organs = new List<string>();
            while (sdr.Read())
            {
                organs.Add(Convert.ToString(sdr["orgname"]));
            }
            ViewData["organs"] = organs;
            ViewData["PageCount"] = Math.Ceiling((decimal)WorkerCount / (decimal)Limit);
            sdr.Close();
            ///////////
            comm.CommandText = "SELECT post.postname FROM post";
            sdr = comm.ExecuteReader();
            List<string> posts = new List<string>();
            while (sdr.Read())
            {
                posts.Add(Convert.ToString(sdr["postname"]));
            }
            ViewData["posts"] = posts;
            ViewData["PageCount"] = Math.Ceiling((decimal)WorkerCount / (decimal)Limit);
            sdr.Close();
            return View(displaytelephone);
        }
    }
}