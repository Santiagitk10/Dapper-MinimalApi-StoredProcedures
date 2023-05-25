if not exists (select 1 from dbo.[User])
begin
	insert into dbo.[User] (FirstName, LastName)
	values ('Santiago', 'Sierra'),
	('Delia', 'Herrera'),
	('Antonio', 'Zapata');
end