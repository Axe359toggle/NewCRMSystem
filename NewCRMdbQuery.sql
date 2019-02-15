drop database NewCRMdb;
create database NewCRMdb;
use NewCRMdb;

create table Location
(
	location_id int identity(10000,1) not null primary key,
	location_name varchar(20),
	location_type varchar(12),
	addr_no varchar(30),
	addr_lane varchar(30),
	addr_town varchar(30),
	addr_city varchar(30),
);

create table Location_tp
(
	location_id int foreign key references Location(location_id),
	location_tp varchar(12),
	primary key (location_id,location_tp)
);

create table Designation
(
	des_id varchar(1) primary key,
	desName varchar(20)
);

insert into Designation (des_id,desName) values ('T','Top Manager') , ('H','Headquarters Manager') , ('S','Showroom Manager') , ('F','Factory Manager');

create table Manager
(
	emp_id int identity(100000,1) not null primary key,
	emp_title varchar(5),
	emp_fname varchar(20),
	emp_lname varchar(20),
	emp_tp varchar(12),
	assigned_dt date Default GETDATE(),
	des_id varchar(1) foreign key references Designation(des_id),
	location_id int foreign key references Location(location_id)
);

create table Login
(
	login_id int identity(100000,1) not null primary key,
	emp_username varchar(30) unique,
	emp_pass varchar(64),
	loginset_dt date default GETDATE() not null,
	account_status bit default 1 not null,
	emp_id int foreign key references Manager(emp_id),
	des_id varchar(1) foreign key references Designation(des_id)
);

/*Adding login ID column to Manager table*/
alter table Manager add login_id int foreign key references Login(login_id);

create table LoginDetails
(/*Test date time */
	logindetail_id int identity(1,1) not null primary key,
	login_dt datetime default GETDATE() not null,
	logout_dt datetime ,
	login_id int foreign key references Login(login_id) not null
);



create table Reference
(
	refID int primary key identity(10000000,1)
);

create table ComplaintStatus
(
	comp_status_id int identity(1,1) not null primary key,
	current_status varchar(50) not null,
	description varchar(50) not null,
);

insert into ComplaintStatus (current_status , description ) VALUES 
('Opened' , 'Item not received' ) , 
('Item received' , 'Rebate not assigned' ) , 
('Rebate assigned' , 'Customer Choice not received' ) ,
('Closed' , 'Customer Choice received - Rebate Accepted' ) ,
('Customer Choice received - Rebate Rejected' , 'Item not sent to HQ' ) , 
('Item sent to HQ' , 'Item not received by HQ' ) , 
('Item received by HQ' , 'Factory not assigned' ) , 
('Factory assigned' , 'Item not sent to Factory' ) , 
('Item sent to Factory' , 'Item not received by Factory' ) , 
( 'Item received by Factory' , 'Factory decision not made' ) , 
('Factory decision made - Repair' , 'Item not repaired' ) , 
('Item repaired' , 'Repaired item not sent to HQ' ) , 
('Repaired item sent to HQ' , 'Repaired item not received by HQ' ) , 
('Repaired item received by HQ' , 'Repaired item not sent to Showroom' ) , 
('Repaired item sent to Showroom' , 'Repaired item not received by Showroom' ) , 
('Repaired item received by Showroom' , 'Repaired item not received by Customer' ) , 
('Closed' , 'Repaired item received by Customer' ) ,
('Factory decision made - Investigation' , 'New item not assigned' ) ,
('New item assigned' , 'New item not sent to Showroom' ) ,
('New item sent to Showroom' , 'New item not received by Showroom' ) ,
('New item received by Showroom' , 'New item not received by Customer' ) ,
('Closed' , 'New item received by Customer' ) ,
('Opened' , 'Staff Complaint details not entered' ) ,
('Staff Complaint details entered' , 'HQ Manager decision not made' ) ,
('Closed' , 'HQ Manager decision made' ) ,
('Opened' , 'Item not received' ) ,
('Item received' , 'Item not sent to HQ' ) ,
('Item sent to HQ' , 'Item not received by HQ' ) ,
('Item received by HQ' , 'Factory not assigned' ) ,
('Factory assigned' , 'Item not sent to Factory' ) ,
('Item sent to Factory' , 'Item not received by Factory' ) ,
('Item received by Factory' , 'Factory decision not made' ) ,
('Factory decision made - Repair' , 'Item not repaired' ) ,
('Item repaired' , 'Repaired item not sent to HQ' ) ,
('Repaired item sent to HQ' , 'Repaired item not received by HQ' ) ,
('Repaired item received by HQ' , 'Repaired item not sent to Showroom' ) ,
('Repaired item sent to Showroom' , 'Repaired item not received by Showroom' ) ,
('Closed' , 'Repaired item received by Showroom' ) ,
('Factory decision made - Investigation' , 'New item not assigned' ) ,
('New item assigned' , 'New item not sent to Showroom' ) ,
('New item sent to Showroom' , 'New item not received by Showroom' ) ,
('Closed' , 'New item received by Showroom' ) ;


create table Complaint
(
	comp_id int identity(10000000,1) not null primary key,
	comp_dt date default GETDATE() not null,
	comp_status_id int DEFAULT 1 references ComplaintStatus(comp_status_id),
	comp_type varchar(8),
	recordedEmp_id int foreign key references Manager(emp_id),
	ref_id int foreign key references Reference(refID),
	relatedLocation_id int foreign key references Location(location_id),
	recordedLocation_id int foreign key references Location(location_id),
	closed_dt date
);

create table Complaint_Reference
(
	ref_id int foreign key references Reference(refID),
	comp_id int foreign key references Complaint(comp_id),
	primary key (ref_id,comp_id)
);


create table ItemType
(
	item_type_id int identity(10000,1) not null primary key,
	item_brand varchar(10),
	item_category varchar(15),
	item_name varchar(50),
	item_size varchar(2),
	item_pic varchar(120),
);

create table Item
(
	item_id varchar(15) primary key,
	item_price numeric(7,2),
	item_type_id int foreign key references ItemType(item_type_id)
);

create table ComplaintItem
(
	comp_item_id int identity(10000000,1) not null primary key,
	shoe_side varchar(5),
	received_dt date default GETDATE(),
	returned_dt date,
	item_defect_img varchar(120),
	item_defect varchar(100),
	item_remarks varchar(250),
	item_decision varchar(13),
	item_id varchar(15) foreign key references Item(item_id),
	item_type_id int foreign key references ItemType(item_type_id),
	comp_id int foreign key references Complaint(comp_id)
);

create table Delivery
(
	delivery_id int identity(100000000,1) not null primary key,
	comp_item_id int foreign key references ComplaintItem(comp_item_id),
	source_id int foreign key references Location(location_id),
	destination_id int foreign key references Location(location_id),
	source_dt date default GETDATE(),
	destination_dt date,
	CHECK (source_id <> destination_id)
);

create table Rebate
(
	comp_item_id int primary key foreign key references ComplaintItem(comp_item_id),
	hQManager int foreign key references Manager(emp_id),
	shrmManager int foreign key references Manager(emp_id),
	rebate_percentage varchar(4),
	customer_choice varchar(8)
);


create table Investigation
(
	comp_item_id int primary key foreign key references ComplaintItem(comp_item_id),
	factoryManager int foreign key references Manager(emp_id),
	investigation_dt date default GETDATE(),
	hQManager int foreign key references Manager(emp_id),
	newItem_id varchar(15) foreign key references Item(item_id)
);

create table Repair
(
	comp_item_id int primary key foreign key references ComplaintItem(comp_item_id),
	factoryManager int foreign key references Manager(emp_id),
	repair_remarks varchar(200),
	repair_dt date
);

create table ManagerComplaint
(
	comp_id int primary key foreign key references Complaint(comp_id),
	comp_item_id int foreign key references ComplaintItem(comp_item_id)
);

create table Customer
(
	cus_id int identity(1000000,1) not null primary key,
	cus_name varchar(50),
	cus_tp varchar(12),
	cus_email varchar(100)
);

create table CustomerComplaint
(
	comp_id int unique foreign key references Complaint(comp_id),
	cus_id int foreign key references Customer(cus_id),
	comp_method varchar(9),
	cus_comp_type varchar(5),
);

create table StaffComplaint
(
	comp_id int primary key foreign key references Complaint(comp_id),
	staff_id varchar(6),
	staff_name varchar(30),
	description varchar(250),
	remarks varchar(250),
	closed_manager int foreign key references Manager(emp_id)
);

CREATE PROCEDURE get_complaint_received_showroom_and_comp_count
AS
SELECT L.location_name , COUNT(C.comp_id) as no_of_comp FROM Location as L , Complaint as C WHERE L.location_id = C.recordedLocation_id GROUP BY L.location_name
;

exec get_complaint_received_showroom_and_comp_count

SELECT L.location_name , COUNT(C.comp_id) as no_of_comp FROM Location as L , Complaint as C WHERE L.location_id = C.recordedLocation_id GROUP BY L.location_name
SELECT L.location_name , MAX( COUNT(C.comp_id) ) as no_of_comp FROM Location as L , Complaint as C WHERE L.location_id = C.recordedLocation_id GROUP BY L.location_name
SELECT L.location_name , COUNT(C.comp_id) as no_of_comp FROM Location as L , Complaint as C WHERE L.location_id = C.recordedLocation_id AND C.comp_type = 'Manager' GROUP BY L.location_name 
SELECT IT.item_brand , COUNT(CI.comp_id) as no_of_comp FROM ItemType as IT , ComplaintItem as CI WHERE IT.item_type_id = CI.item_type_id GROUP BY IT.item_brand 
SELECT IT.item_category , COUNT(CI.comp_id) as no_of_comp FROM ItemType as IT , ComplaintItem as CI WHERE IT.item_type_id = CI.item_type_id GROUP BY IT.item_category




insert into Manager(emp_fname) values ('Admin');
insert into Login (emp_username,emp_pass,des_id,emp_id) values ('Admin','8baefafe1f22ce4cdf896b0f7d5ee0a45d2c7a678e74f86986148af497e9554c','H',100000);

/*Test Data*/
INSERT INTO location ( location_name,location_type,addr_no ,addr_lane,addr_town,addr_city)	VALUES('Galle_01','Showroom','12','TB Jayah Mawatha','Batawatta','Galle');
INSERT INTO location ( location_name,location_type,addr_no ,addr_lane,addr_town)			VALUES('Colombo_01','HQ','188/1/A','Stanley Thilakarathne Mawatha','Nugegoda');
INSERT INTO location ( location_name,location_type,addr_no ,addr_lane,addr_town)			VALUES('Kandy','Factory','200/1A','Galle Road','Moratuwa');
INSERT INTO location ( location_name,location_type,addr_no ,addr_lane,addr_town,addr_city)	VALUES('Trincomalee','Factory','85','N.C','Nilaveli Road', 'Trincomalee');
INSERT INTO location ( location_name,location_type,addr_no ,addr_lane,addr_town,addr_city)	VALUES('Jaffna_01','Showroom','32/2/4B','Sea Line','Colombo Road', 'Jaffna');
INSERT INTO location ( location_name,location_type,addr_no ,addr_lane)						VALUES('Matara_01','Showroom','Nilwala By Pass','Matara');
INSERT INTO location ( location_name,location_type,addr_no ,addr_lane)						VALUES('Kurunegala_01','Showroom', 'Parakumba Street', 'Kurunegala');
INSERT INTO location ( location_name,location_type,addr_no ,addr_lane,addr_town)			VALUES('Kurunegala','Factory','Dangaspitiya Industrial State','Kohilegedara',' Kurunegala');
INSERT INTO location ( location_name,location_type,addr_no ,addr_lane)						VALUES('Anuradhapura_01','Showroom','Mithreepala Senanayake Mawatha', 'Anuradhapura');
INSERT INTO location ( location_name,location_type,addr_no ,addr_lane)						VALUES('Ratmalana','Factory','Mount Lavinia Road', 'Ratmalana');

INSERT INTO location_tp(location_id,location_tp) VALUES(10000,+94912291100);
INSERT INTO location_tp(location_id,location_tp) VALUES(10001,+94112003334);
INSERT INTO location_tp(location_id,location_tp) VALUES(10002,+94812222261);
INSERT INTO location_tp(location_id,location_tp) VALUES(10003,+94262222432);
INSERT INTO location_tp(location_id,location_tp) VALUES(10004,+94212222334);
INSERT INTO location_tp(location_id,location_tp) VALUES(10005,+94412222261);
INSERT INTO location_tp(location_id,location_tp) VALUES(10006,+94372222261);
INSERT INTO location_tp(location_id,location_tp) VALUES(10007,+94372233261);
INSERT INTO location_tp(location_id,location_tp) VALUES(10008,+94252222223);
INSERT INTO location_tp(location_id,location_tp) VALUES(10009,+94112738351);


INSERT INTO Customer (cus_name, cus_tp, cus_email)	VALUES('Asiri Ranathunge','+94912291100', 'asiri@gmail.com');
INSERT INTO Customer (cus_name, cus_tp, cus_email)	VALUES('Maneesha Rajapakshe','+94114052323', 'maneesha@gmail.com');
INSERT INTO Customer (cus_name, cus_tp, cus_email)	VALUES('Bhanuke Perera','+94382215632', 'bhanuka@gmail.com');
INSERT INTO Customer (cus_name, cus_tp, cus_email)	VALUES('Chaminda Perera','+94812234567', 'chaminda@gmail.com');
INSERT INTO Customer (cus_name, cus_tp, cus_email)	VALUES('Athula Ranasinghe','+94114003232', 'athula@gmail.com');

INSERT INTO Manager (emp_title, emp_fname,emp_lname, emp_tp,assigned_dt,des_id, location_id)	VALUES('Mrs.','Asiri',' Ranathunge','+94912291100','2019-01-17','S',10003); --10000   ,100001
INSERT INTO Manager (emp_title, emp_fname,emp_lname, emp_tp,assigned_dt,des_id, location_id)	VALUES('Mr.','Matheesha','Rajapakshe','+94712074179','2019-01-17','H',10004); --10001	 ,100002
INSERT INTO Manager (emp_title, emp_fname,emp_lname, emp_tp,assigned_dt,des_id, location_id)	VALUES('Ms.','Maneesha',' Siriwardhene','+94812222261','2019-01-17','F',10005);--10002; ,100003
INSERT INTO Manager (emp_title, emp_fname,emp_lname, emp_tp,assigned_dt,des_id, location_id)	VALUES('Mr.','Bhanuka','Ekanayake','+94262222432','2019-01-17','F',10006);	   --10003	 ,100004
INSERT INTO Manager (emp_title, emp_fname,emp_lname, emp_tp,assigned_dt,des_id, location_id)	VALUES('Mrs.','Mithila',' Galappatti','+94212223348','2019-01-17','S',10007);  --10004	 ,100005
INSERT INTO Manager (emp_title, emp_fname,emp_lname, emp_tp,assigned_dt,des_id, location_id)	VALUES('Mr.','Nuwan',' Perera','+94412222261','2019-01-17','S',10008);		  --10005	 ,100006
INSERT INTO Manager (emp_title, emp_fname,emp_lname, emp_tp,assigned_dt,des_id, location_id)	VALUES('Ms.','Prashansani','Fernando','+94372222261','2019-01-17','S',10009); --10006	,100007
INSERT INTO Manager (emp_title, emp_fname,emp_lname, emp_tp,assigned_dt,des_id, location_id)	VALUES('Mr.','Kasun','Perera','+94372233261','2019-01-17','F',10010);		   --10007	,100008
INSERT INTO Manager (emp_title, emp_fname,emp_lname, emp_tp,assigned_dt,des_id, location_id)	VALUES('Mrs.','Jithmi',' Athukorala','+94252222223','2019-01-17','S',10011);   --10008	,100009
INSERT INTO Manager (emp_title, emp_fname,emp_lname, emp_tp,assigned_dt,des_id, location_id)	VALUES('Mr.','Mandara',' Nanayakkara','+94112738351','2019-01-17','F',10012);  --10009	, 100010

INSERT INTO Login(emp_username,emp_pass,loginset_dt,account_status,emp_id,des_id)	VALUES('User1','8baefafe1f22ce4cdf896b0f7d5ee0a45d2c7a678e74f86986148af497e9554c','2018-01-17',1,100004,'S'); --100001
INSERT INTO Login(emp_username,emp_pass,loginset_dt,account_status,emp_id,des_id)	VALUES('User2','8baefafe1f22ce4cdf896b0f7d5ee0a45d2c7a678e74f86986148af497e9554c','2018-01-17',1,100005,'H'); --100002
INSERT INTO Login(emp_username,emp_pass,loginset_dt,account_status,emp_id,des_id)	VALUES('User3','8baefafe1f22ce4cdf896b0f7d5ee0a45d2c7a678e74f86986148af497e9554c','2018-01-17',1,100006,'F'); --100003
INSERT INTO Login(emp_username,emp_pass,loginset_dt,account_status,emp_id,des_id)	VALUES ('User4','8baefafe1f22ce4cdf896b0f7d5ee0a45d2c7a678e74f86986148af497e9554c','2018-01-17',1,100007,'F'); --100004
INSERT INTO Login(emp_username,emp_pass,loginset_dt,account_status,emp_id,des_id)	VALUES ('User5','8baefafe1f22ce4cdf896b0f7d5ee0a45d2c7a678e74f86986148af497e9554c','2018-01-17',1,100008,'S'); --100005
INSERT INTO Login(emp_username,emp_pass,loginset_dt,account_status,emp_id,des_id)	VALUES('User6','8baefafe1f22ce4cdf896b0f7d5ee0a45d2c7a678e74f86986148af497e9554c','2018-01-17',1,100009,'S'); --100006
INSERT INTO Login(emp_username,emp_pass,loginset_dt,account_status,emp_id,des_id)	VALUES('User7','8baefafe1f22ce4cdf896b0f7d5ee0a45d2c7a678e74f86986148af497e9554c','2018-01-17',1,100010,'S'), --100007
																							('User8','8baefafe1f22ce4cdf896b0f7d5ee0a45d2c7a678e74f86986148af497e9554c','2018-01-17',1,100011,'F'), --100008
																							('User9','8baefafe1f22ce4cdf896b0f7d5ee0a45d2c7a678e74f86986148af497e9554c','2018-01-17',1,100012,'S'), --100009
																							('User10','8baefafe1f22ce4cdf896b0f7d5ee0a45d2c7a678e74f86986148af497e9554c','2018-01-17',1,100013,'F'); --100010
UPDATE Manager SET login_id = 100004 WHERE emp_id = 100004;
UPDATE Manager SET login_id = 100005 WHERE emp_id = 100005;
UPDATE Manager SET login_id = 100006 WHERE emp_id = 100006;
UPDATE Manager SET login_id = 100007 WHERE emp_id = 100007;
UPDATE Manager SET login_id = 100008 WHERE emp_id = 100008;
UPDATE Manager SET login_id = 100009 WHERE emp_id = 100009;
UPDATE Manager SET login_id = 100010 WHERE emp_id = 100010;
UPDATE Manager SET login_id = 100011 WHERE emp_id = 100011;
UPDATE Manager SET login_id = 100012 WHERE emp_id = 100012;
UPDATE Manager SET login_id = 100013 WHERE emp_id = 100013;


INSERT INTO ItemType(item_brand,item_category,item_name,item_size) VALUES('jbc','MenShoe','MSqwerty001','M');
INSERT INTO ItemType(item_brand,item_category,item_name,item_size) VALUES('BackStage','LadiesShoe','LSqwerty001','L');
INSERT INTO ItemType(item_brand,item_category,item_name,item_size) VALUES('Waves','MenShoe','MSqwerty002','38');
INSERT INTO ItemType(item_brand,item_category,item_name,item_size) VALUES('Bumpers','LadiesShoe','LSqwerty002','S');
INSERT INTO ItemType(item_brand,item_category,item_name,item_size) VALUES('samsons','LadiesShoe','LSqwerty003','L');
INSERT INTO ItemType(item_brand,item_category,item_name,item_size) VALUES('FILA','MenShoe','MSqwerty003','37');
INSERT INTO ItemType(item_brand,item_category,item_name,item_size) VALUES('Beat','LadiesShoe','LSqwerty004','35');
INSERT INTO ItemType(item_brand,item_category,item_name,item_size) VALUES('FatFace','MenShoe','MSqwerty004','39');
INSERT INTO ItemType(item_brand,item_category,item_name,item_size) VALUES('SweetBetty','LadiesShoe','LSqwerty005','40');
INSERT INTO ItemType(item_brand,item_category,item_name,item_size) VALUES('FashionMov','LadiesShoe','LSqwerty006','S');

/*
Insert Into Item (item_id ,item_price,item_type_id )	values ('GF0084-07-ASSTD',2000.00,10000),
																('GF0085-08-ASSTD',2000.00,10001),
																('GF0086-08-ASSTD',2500.00,10002),
																('GF0087-08-ASSTD',3500.00,10003),
																('GF0088-08-ASSTD',3000.00,10004),
																('GF0089-08-ASSTD',2000.00,10005),
																('GF0090-08-ASSTD',2500.00,10006),
																('GF0091-08-ASSTD',3500.00,10007),
																('GF0092-08-ASSTD',3000.00,10008);
																*/


/*
/*Customer Complaint*/
INSERT INTO Reference DEFAULT VALUES DECLARE @refID int = SCOPE_IDENTITY();
INSERT INTO Complaint (comp_type , ref_id , relatedLocation_id , comp_status_id , recordedEmp_id , recordedLocation_id , comp_dt) VALUES ('" + compType1 + "', @refID ,'" + relShrmID + "' , " + compStatusID + " , " + Login.EmpID + " , " + Login.LocID + " , 'comp_dt') DECLARE @compID int = SCOPE_IDENTITY() INSERT INTO CustomerComplaint (comp_id,cus_id,comp_method,cus_comp_type) values(@compID,'" + cusID + "','" + compMethod + "','" + compType2 + "');

/*Set Staff Complaint*/
INSERT INTO StaffComplaint (comp_id ,staff_id ,staff_name ,description ) VALUES (@compID , '"+staffID+"' , '"+staffName+"' , '"+description+"');
UPDATE Complaint SET comp_status_id = 24 WHERE comp_id = @compID ;

/*Close Staff Complaint*/
DECLARE @COMPstatusID1 int SET @COMPstatusID1 = 25;
UPDATE Complaint SET comp_status_id = @COMPstatusID1 , closed_dt = GETDATE() WHERE comp_id = @compID;


/*Record received Item */
INSERT INTO Item (item_id ,item_price , item_type_id ) VALUES ( '" + itemID + "' , " + itemPrice + " , '" + itemTypeID + "' ) ;
DECLARE @COMPstatusID2 int SET @COMPstatusID2 = (select case when comp_status_id = 1 then 2 when comp_status_id = 26 then 27 END as comp_status_id from Complaint WHERE comp_id = @compID) ;
UPDATE Complaint SET comp_status_id = @COMPstatusID2 WHERE comp_id = @compID ;
INSERT INTO ComplaintItem ( shoe_side , received_dt , item_defect , item_remarks , item_id , item_type_id ,comp_id ) VALUES ( '"+shoeSide+"' , '"+receivedDt+"' , '"+itemDefect+"' , '"+itemRemarks+"' , '"+itemID+"' , '"+itemTypeID+"' , @compID ) DECLARE @COMPitemID int = SCOPE_IDENTITY();
Update ComplaintItem SET item_defect_img = '" + imagePath + "' WHERE comp_item_id = @COMPitemID ;


/*Assign Rebate*/
DECLARE @COMPitemID int SET @COMPitemID = (SELECT CI.comp_item_id FROM ComplaintItem CI WHERE CI.comp_id = '" + compID + "')  INSERT INTO Rebate (comp_item_id ,hQManager ,rebate_percentage ) VALUES (@COMPitemID," + HQManagerID + ",'" + rebatePercentage + "') ;
UPDATE Complaint SET comp_status_id = 3 WHERE comp_id = " + compID + " ;

/*Rebate Payment*/
DECLARE @ID int SET @ID = (SELECT CI.comp_item_id FROM ComplaintItem CI WHERE CI.comp_id = '" + compID + "') ;
Update Rebate SET shrmManager = " + ShowroomManagerID + " , customer_choice = '" + cusChoice + "' WHERE comp_item_id = @ID ;
UPDATE Complaint SET comp_status_id = " + compStatusID + " WHERE comp_id = @compID ;
--UPDATE Complaint SET closed_dt = GETDATE() WHERE comp_id = @compID ;

/*Deliver Item*/
DECLARE @COMPitemID int SET @COMPitemID = (SELECT CI.comp_item_id FROM ComplaintItem CI WHERE CI.comp_id = '" + compID + "') INSERT INTO Delivery ( comp_item_id , source_id , destination_id , source_dt) VALUES ( @COMPitemID , '" + sourceID + "' , '" + destinationID + "' , '" + sourceDt + "' )  DECLARE @ID int = SCOPE_IDENTITY() SELECT @ID as delivery_id ;
DECLARE @COMPstatusID int SET @COMPstatusID = (select case when comp_status_id = 5 then 6 when comp_status_id = 27 then 28 when comp_status_id = 8 then 9 when comp_status_id = 30 then 31 when comp_status_id = 12 then 13 when comp_status_id = 34 then 35 when comp_status_id = 14 then 15 when comp_status_id = 36 then 37 when comp_status_id = 19 then 20 when comp_status_id = 40 then 41 END as comp_status_id from Complaint WHERE comp_id = '" + compID + "') ;
UPDATE Complaint SET comp_status_id = @COMPstatusID WHERE comp_id = '" + compID + "' ;

/*Record Delivered Item*/
UPDATE Delivery SET destination_dt = '" + desDt + "' WHERE delivery_id = '" + deliveryID + "';
DECLARE @COMPstatusID int SET @COMPstatusID = (select case when comp_status_id = 6 then 7 when comp_status_id = 28 then 29 when comp_status_id = 9 then 10 when comp_status_id = 31 then 32 when comp_status_id = 13 then 14 when comp_status_id = 35 then 36 when comp_status_id = 15 then 16 when comp_status_id = 37 then 38 when comp_status_id = 20 then 21 when comp_status_id = 41 then 42 END as comp_status_id from Complaint WHERE comp_id = '" + compID + "');
UPDATE Complaint SET comp_status_id = @COMPstatusID WHERE comp_id = '" + compID + "' SELECT @COMPstatusID as comp_status_id , L.location_id , L.location_name FROM Location as L , Complaint as C WHERE C.comp_id = '" + compID + "' AND C.recordedLocation_id = L.location_id ;

/*Assign Factory*/
DECLARE @COMPstatusID int SET @COMPstatusID = (SELECT CASE WHEN C.comp_status_id = 7 THEN 8 WHEN C.comp_status_id = 29 THEN 30 END FROM Complaint C WHERE C.comp_id = '" + compID + "');
UPDATE Complaint SET comp_status_id = @COMPstatusID WHERE comp_id = '" + compID + "' ;

/*Investigation*/
DECLARE @COMPitemID int SET @COMPitemID = (SELECT CI.comp_item_id FROM ComplaintItem CI WHERE CI.comp_id = '" + compID + "') INSERT INTO Investigation (comp_item_id , factoryManager , investigation_dt ) VALUES ( @COMPitemID , " + facManagerID + " , '" + invDate + "' ) UPDATE ComplaintItem SET item_decision = 'Investigation' WHERE comp_item_id = @COMPitemID ;
DECLARE @COMPstatusID int SET @COMPstatusID = (select case when comp_status_id = 10 then 18 when comp_status_id = 32 then 39 END as comp_status_id from Complaint WHERE comp_id = '" + compID + "');
UPDATE Complaint SET comp_status_id = @COMPstatusID WHERE comp_id = '" + compID + "';

/*Repair*/
DECLARE @COMPitemID int SET @COMPitemID = (SELECT CI.comp_item_id FROM ComplaintItem CI WHERE CI.comp_id = '" + compID + "') INSERT INTO Repair (comp_item_id , factoryManager ) VALUES ( @COMPitemID , " + facManagerID + " ) UPDATE ComplaintItem SET item_decision = 'Repair' WHERE comp_item_id = @COMPitemID ;
DECLARE @COMPstatusID int SET @COMPstatusID = (select case when comp_status_id = 10 then 11 when comp_status_id = 32 then 33 END as comp_status_id from Complaint WHERE comp_id = '" + compID + "') ;
UPDATE Complaint SET comp_status_id = @COMPstatusID WHERE comp_id = '" + compID + "';

/*Repair Details*/
DECLARE @COMPitemID int SET @COMPitemID = (SELECT CI.comp_item_id FROM ComplaintItem CI WHERE CI.comp_id = '" + compID + "') Update Repair SET repair_remarks = '" + repairRemarks + "' , repair_dt = '" + repairedDate + "' WHERE comp_item_id = @COMPitemID  ;
DECLARE @COMPstatusID int SET @COMPstatusID = (select case when comp_status_id = 11 then 12 when comp_status_id = 33 then 34 END as comp_status_id from Complaint WHERE comp_id = '" + compID + "') ;
UPDATE Complaint SET comp_status_id = @COMPstatusID WHERE comp_id = '" + compID + "' ;

/*Deliver Item to Customer */
DECLARE @COMPitemID int SET @COMPitemID = (SELECT CI.comp_item_id FROM ComplaintItem CI WHERE CI.comp_id = '" + compID + "') ;
UPDATE ComplaintItem SET returned_dt = GETDATE() WHERE comp_item_id = @COMPitemID ;
DECLARE @COMPstatusID int SET @COMPstatusID = (select case when comp_status_id = 16 then 17 when comp_status_id = 21 then 22 END as comp_status_id from Complaint WHERE comp_id = '" + compID + "') ;
UPDATE Complaint SET comp_status_id = @COMPstatusID , closed_dt = GETDATE() WHERE comp_id = '" + compID + "';


/*Assign New Item*/
INSERT INTO Item (item_id , item_type_id ) VALUES ( '" + newItemID + "' , '" + itemTypeID + "' ) ;
DECLARE @COMPitemID int SET @COMPitemID = (SELECT CI.comp_item_id FROM ComplaintItem CI WHERE CI.comp_id = '" + compID + "') UPDATE Investigation SET hQManager = " + Login.EmpID + " , newItem_id = '" + newItemID + "' WHERE comp_item_id = @COMPitemID ;
DECLARE @COMPstatusID int SET @COMPstatusID = (select case when comp_status_id = 18 then 19 when comp_status_id = 39 then 40 END as comp_status_id from Complaint WHERE comp_id = '" + compID + "') ;
UPDATE Complaint SET comp_status_id = @COMPstatusID WHERE comp_id = '" + compID + "' ;
SELECT L.location_id , L.location_name FROM Location AS L , Complaint AS C WHERE C.comp_id = '" + compID + "' AND C.recordedLocation_id = L.location_id ;



/*Manager Complaint*/
INSERT INTO Reference DEFAULT VALUES DECLARE @ID int = SCOPE_IDENTITY() SELECT @ID as ref_id;
INSERT INTO Complaint (comp_type , ref_id , relatedLocation_id , comp_status_id , recordedEmp_id , recordedLocation_id) VALUES ('" + compType1 + "','" + refID + "','" + relShrmID + "' , " + compStatusID + " , " + Login.EmpID + " , " + Login.LocID + ") DECLARE @ID int = SCOPE_IDENTITY() SELECT @ID as comp_id;

*/

/*
/* CUSTOMER ITEM COMPLAINT ENDS IN REBATE */
--Complained Date : 2019-02-04
--Final CompStatus = 4
/*Customer Complaint*/
INSERT INTO Reference DEFAULT VALUES DECLARE @refID int = SCOPE_IDENTITY();
INSERT INTO Complaint (comp_type , ref_id , relatedLocation_id , comp_status_id , recordedEmp_id , recordedLocation_id,comp_dt) VALUES ('Customer', @refID ,'10003' , 1 , 100004 , 10003 , '2019-02-04') DECLARE @compID int = SCOPE_IDENTITY() INSERT INTO CustomerComplaint (comp_id,cus_id,comp_method,cus_comp_type) values(@compID,'1002001','By Call','Item');

/*Record received Item */
DECLARE @itemID varchar(15) SET @itemID = 325689745123658;
DECLARE @itemTypeID varchar(5) SET @itemTypeID = 10003;
INSERT INTO Item (item_id ,item_price , item_type_id ) VALUES ( @itemID , 5600.00 , @itemTypeID ) ;
DECLARE @COMPstatusID2 int SET @COMPstatusID2 = (select case when comp_status_id = 1 then 2 when comp_status_id = 26 then 27 END as comp_status_id from Complaint WHERE comp_id = @compID) ;
UPDATE Complaint SET comp_status_id = @COMPstatusID2 WHERE comp_id = @compID ;
INSERT INTO ComplaintItem ( shoe_side , received_dt , item_defect , item_remarks , item_id , item_type_id ,comp_id ) VALUES ( 'Both' , '2019-02-05' , 'Damaged Sole' , 'Item in good condition' , @itemID , @itemTypeID , @compID ) DECLARE @COMPitemID int = SCOPE_IDENTITY();
--Update ComplaintItem SET item_defect_img = '" + imagePath + "' WHERE comp_item_id = @COMPitemID ;

/*Assign Rebate*/
SET @COMPitemID = (SELECT CI.comp_item_id FROM ComplaintItem CI WHERE CI.comp_id = @compID)  INSERT INTO Rebate (comp_item_id ,hQManager ,rebate_percentage ) VALUES (@COMPitemID, 100005 ,'45%') ;
UPDATE Complaint SET comp_status_id = 3 WHERE comp_id = @compID ;

/*Rebate Payment*/
Update Rebate SET shrmManager = 100004 , customer_choice = 'Accepted' WHERE comp_item_id = @COMPitemID ;
UPDATE Complaint SET comp_status_id = 4 WHERE comp_id = @compID ;
UPDATE Complaint SET closed_dt = '2019-02-05' WHERE comp_id = @compID ;

*/





/*
select * from Login;
select * from LoginDetails;
select * from Manager;
select * from Location;
select * from item;
select * from itemType;
select * from Complaint;
select * from Customer;
select * from complaintitem;
select * from Rebate;
select * from Investigation;
select * from ComplaintStatus;
select M.emp_tp from Manager as M , Login as L WHERE M.login_id = L.login_id AND L.account_status = 1 AND M.des_id = 'H';
SELECT delivery_id FROM Delivery WHERE destination_dt IS NULL;
select C.* , CS.current_status , CS.description , CI.comp_item_id  from Complaint as C , ComplaintStatus as CS , ComplaintItem as CI WHERE C.comp_status_id = CS.comp_status_id AND C.comp_id = CI.comp_id;
SELECT TOP 1 L.location_id , L.location_name FROM Location AS L WHERE location_type = 'HQ';
SELECT C.comp_id , C.comp_dt , C.ref_id , C.comp_type , CS.current_status , CS.description , C.recordedEmp_id , C.relatedLocation_id , C.recordedLocation_id , C.closed_dt FROM Complaint as C , ComplaintStatus as CS WHERE C.comp_status_id = CS.comp_status_id  AND ( C.comp_type LIKE '%Cus%')


/*Delivery initiation*/
SELECT CASE 
WHEN comp_status_id = 5 THEN 6 
WHEN comp_status_id = 27 THEN 28 
WHEN comp_status_id = 8 THEN 9
WHEN comp_status_id = 30 THEN 31
WHEN comp_status_id = 12 THEN 13
WHEN comp_status_id = 34 THEN 35
WHEN comp_status_id = 14 THEN 15
WHEN comp_status_id = 36 THEN 37
WHEN comp_status_id = 19 THEN 20
WHEN comp_status_id = 40 THEN 41
END AS comp_status_id FROM Complaint;

select case when comp_status_id = 5 then 6 when comp_status_id = 27 then 28 when comp_status_id = 8 then 9 when comp_status_id = 30 then 31 when comp_status_id = 12 then 13 when comp_status_id = 34 then 35 when comp_status_id = 14 then 15 when comp_status_id = 36 then 37 when comp_status_id = 19 then 20 when comp_status_id = 40 then 41 END as comp_status_id from Complaint;


/*Delivery termination*/
SELECT CASE 
WHEN comp_status_id = 6 THEN 7 
WHEN comp_status_id = 28 THEN 29 
WHEN comp_status_id = 9 THEN 10 
WHEN comp_status_id = 31 THEN 32 
WHEN comp_status_id = 13 THEN 14 
WHEN comp_status_id = 35 THEN 36 
WHEN comp_status_id = 15 THEN 16 
WHEN comp_status_id = 37 THEN 38 
WHEN comp_status_id = 20 THEN 21 
WHEN comp_status_id = 41 THEN 42 
END AS comp_status_id FROM Complaint;

select case when comp_status_id = 6 then 7 when comp_status_id = 28 then 29 when comp_status_id = 9 then 10 when comp_status_id = 31 then 32 when comp_status_id = 13 then 14 when comp_status_id = 35 then 36 when comp_status_id = 15 then 16 when comp_status_id = 37 then 38 when comp_status_id = 20 then 21 when comp_status_id = 41 then 42 END as comp_status_id from Complaint;


*/

--SELECT D.delivery_id , CI.comp_id , D.source_id ,(SELECT location_name FROM Location as L WHERE D.source_id = L.location_id) as source_name , D.destination_id , (SELECT location_name FROM Location as L WHERE D.destination_id = L.location_id) as destination_name  , D.source_dt , D.destination_dt from Delivery as D , ComplaintItem as CI WHERE D.comp_item_id = CI.comp_item_id 