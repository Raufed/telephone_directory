DROP TABLE IF EXISTS organ CASCADE;
DROP TABLE IF EXISTS worker CASCADE;
DROP TABLE IF EXISTS post CASCADE;
--DROP TABLE IF EXISTS Department CASCADE;
DROP TABLE IF EXISTS Telephone CASCADE;
DROP TABLE IF EXISTS telcode CASCADE;

CREATE TABLE IF NOT EXISTS organ(

	idOrgan serial PRIMARY KEY,
	orgName text NOT NULL,
	address text NOT NULL
);
--CREATE TABLE Department(	idDep serial PRIMARY KEY,depName text NOT NULL);
CREATE TABLE post(
	idPost serial PRIMARY KEY,
	postName text NOT NULL
);

CREATE TABLE IF NOT EXISTS worker(

	idWorker serial PRIMARY KEY,
	firstNAme text NOT NULL,
	secondName text NOT NULL,
	patronymic text NOT NULL,	
	fk_organ_worker int REFERENCES organ(idOrgan),
	--fk_dep_worker int REFERENCES Department(idDep),
	fk_post_worker int REFERENCES post(idPost)
);
CREATE TABLE Telephone(

	idTel serial PRIMARY KEY NOT NULL,
	telephone text,
	longTelephone text,
	fk_telephone_worker int REFERENCES worker(idWorker)
);

CREATE TABLE telCode(
	
	idTelCode serial PRIMARY KEY,
	code text NOT NULL,
	fk_telCode_organ int REFERENCES organ(idOrgan)
);

SELECT firstname,secondname,patronymic,organ.orgname,postname,code,telephone.telephone,longtelephone
FROM WORKER 
--INNER JOIN department ON (fk_dep_worker = department.iddep) 
INNER JOIN organ ON (fk_organ_worker = organ.idorgan) 
INNER JOIN telephone ON(idworker = fk_telephone_worker) 
INNER JOIN post ON(idpost = fk_post_worker) 
INNER JOIN telcode ON(idtelcode = idorgan)

--LIMIT 3 OFFSET 0
--SELECT *FROM worker
