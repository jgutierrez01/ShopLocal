
/****** Object:  StoredProcedure [dbo].[SPReturnThreeFields]    Script Date: 02/21/2012 16:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SPFillSegments] 

AS
BEGIN
    declare @strOriginal NVARCHAR(max), @str NVARCHAR(max), @f1 varchar(max), @f2 varchar(max), @f3 varchar(max), @f4 varchar(max),@f5 varchar(max),@f6 varchar(max),@f7 varchar(max), @bool int = 0, @id int;
	
	declare SpoolCursor Cursor for
	select spoolid, nombre from dbo.Spool where segmento1 = ''
	
	open SpoolCursor
	Fetch next from SpoolCursor into @id, @str
	
	WHILE (@@FETCH_STATUS = 0)
	
	begin
	set @strOriginal = @str;
    -- Field 1
    set @f1=(left(@str,CHARINDEX('-',@str,1)-1));
    SET @str=RIGHT(@str,LEN(@str)-CHARINDEX('-',@str,1));

    -- Field 2
    set @f2=(left(@str,CHARINDEX('-',@str,1)-1));
	SET @str=RIGHT(@str,LEN(@str)-CHARINDEX('-',@str,1));
	
    -- Field 3
    set @f3 = (left(@str,CHARINDEX('-',@str,1)-1));
    SET @str=RIGHT(@str,LEN(@str)-CHARINDEX('-',@str,1));
    
    set @f4 = (left(@str,CHARINDEX('-',@str,1)-1));
    SET @str=RIGHT(@str,LEN(@str)-CHARINDEX('-',@str,1));
    
    set @f5 = (left(@str,CHARINDEX('-',@str,1)-1));
    
    set @bool = case when patindex('%' + '-' + '%' , @str) <> 0 then 0 else 1 end;
    SET @str= case when @bool = 0 then RIGHT(@str,LEN(@str)-CHARINDEX('-',@str,1)) else @str end;    
    
    set @f6 = case when patindex('%' + '-' + '%' , @str) < 1 then case when @bool = 0 then @str else '' end else (left(@str,CHARINDEX('-',@str,1)-1)) end;   
    
    set @bool = case when patindex('%' + '-' + '%' , @str) <> 0 then 0 else 1 end;
    SET @str= case when @bool = 0 then RIGHT(@str,LEN(@str)-CHARINDEX('-',@str,1)) else @str end;        
    
    set @f7 = case when patindex('%' + '-' + '%' , @str) < 1 then case when @bool = 0 then @str else '' end else (left(@str,CHARINDEX('-',@str,1)-1)) end;    

    --update dbo.Spool SET Segmento1 = @f1,Segmento2 = @f2,Segmento3 = @f3,Segmento4 = @f4, Segmento5 = @f5, Segmento6 = @f6, Segmento7 = @f7 where Nombre = @strOriginal;
    select @f1,@f2,@f3,@f4,@f5,@f6,@f7 from spool where SpoolID = @id;
   
    Fetch next from  SpoolCursor into @id, @str
    
    end
    
    close SpoolCursor
    
    
    DEALLOCATE  SpoolCursor
    
END


GO


