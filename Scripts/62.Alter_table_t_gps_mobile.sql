ALTER TABLE t_gps_mobile
add address varchar(800) null

ALTER TABLE t_gps_mobile
add createdDate datetime null default getdate()

ALTER TABLE t_gps_mobile
add updatedDate datetime null default getdate()