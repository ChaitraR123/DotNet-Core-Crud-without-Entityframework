
CREATE PROCEDURE BookCreateorEdit
	@bookid int,
	@author varchar(100),
	@title varchar(100),
	@Price int
AS
BEGIN
	if @bookid=0
	begin
	insert into books(Title,Author,Price) values(@title,@author,@Price)
	end
	else
	begin
	update Books set Title=@title,Author=@author,Price=@Price where BookId=@bookid
	end
END
GO


CREATE PROCEDURE ViewAllBooks
	
AS
BEGIN
	select * from Books
END
GO


CREATE PROCEDURE Viewbookbyid
@bookid int	
AS
BEGIN
	select * from Books where BookId=@bookid
END
GO


CREATE PROCEDURE Deletebookbyid
@bookid int	
AS
BEGIN
	delete Books where BookId=@bookid
END
GO

