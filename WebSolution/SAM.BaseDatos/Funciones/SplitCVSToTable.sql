if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SplitCVSToTable]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[SplitCVSToTable]
GO 
create function dbo.SplitCVSToTable
(
 @String nvarchar(MAX),
 @Delimiter nvarchar (10)
 )
returns @ValueTable table ([Value] nvarchar(MAX))

begin

 declare @NextString nvarchar(MAX)
 declare @Pos int
 declare @NextPos int
 declare @CommaCheck nvarchar(1)

if (@String != '' AND @String is not null)
BEGIN

 --Initialize
 set @NextString = ''
 set @CommaCheck = right(@String,1) 
 
 --Check for trailing Comma, if not exists, INSERT
 --if (@CommaCheck <> @Delimiter )
 set @String = @String + @Delimiter
 
 --Get position of first Comma
 set @Pos = charindex(@Delimiter,@String)
 set @NextPos = 1
 
 --Loop while there is still a comma in the String of levels
 while (@pos <>  0)  
 begin
  set @NextString = substring(@String,1,@Pos - 1)
 
  insert into @ValueTable ( [Value]) Values (@NextString)
 
  set @String = substring(@String,@pos +1,len(@String))
  
  set @NextPos = @Pos
  set @pos  = charindex(@Delimiter,@String)
 end
 
END

 return
end

GO



