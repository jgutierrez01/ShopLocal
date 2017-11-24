update JuntaSpool
set EsManual = 1
where FabAreaID = 2 /* FIELD */

delete from
BastonSpoolJunta
where JuntaSpoolID IN (select JuntaSpoolID
                       from JuntaSpool
                       where FabAreaID = 2)
                       

delete from
BastonSpool
where BastonSpoolID NOT IN (select BastonSpoolID
                            from BastonSpoolJunta)