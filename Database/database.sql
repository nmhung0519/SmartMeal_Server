/*
MySQL Backup
Database: smartmeal
Backup Time: 2021-04-26 01:05:58
*/

SET FOREIGN_KEY_CHECKS=0;
DROP TABLE IF EXISTS `smartmeal`.`tbl_log`;
DROP TABLE IF EXISTS `smartmeal`.`tbl_logtype`;
DROP TABLE IF EXISTS `smartmeal`.`tbl_order`;
DROP TABLE IF EXISTS `smartmeal`.`tbl_orderstatus`;
DROP TABLE IF EXISTS `smartmeal`.`tbl_product`;
DROP TABLE IF EXISTS `smartmeal`.`tbl_productline`;
DROP TABLE IF EXISTS `smartmeal`.`tbl_roletype`;
DROP TABLE IF EXISTS `smartmeal`.`tbl_table`;
DROP TABLE IF EXISTS `smartmeal`.`tbl_user`;
DROP PROCEDURE IF EXISTS `smartmeal`.`sp_Order_Insert`;
DROP PROCEDURE IF EXISTS `smartmeal`.`sp_Order_UpdateStatus`;
DROP PROCEDURE IF EXISTS `smartmeal`.`sp_ProductLine_Get`;
DROP PROCEDURE IF EXISTS `smartmeal`.`sp_Product_ChangeStatus`;
DROP PROCEDURE IF EXISTS `smartmeal`.`sp_Product_Insert`;
DROP PROCEDURE IF EXISTS `smartmeal`.`sp_Product_Search`;
DROP PROCEDURE IF EXISTS `smartmeal`.`sp_Table_ChangeName`;
DROP PROCEDURE IF EXISTS `smartmeal`.`sp_Table_GetAllActive`;
DROP PROCEDURE IF EXISTS `smartmeal`.`sp_Table_GetById`;
DROP PROCEDURE IF EXISTS `smartmeal`.`sp_Table_Insert`;
DROP PROCEDURE IF EXISTS `smartmeal`.`sp_User_CheckLogin`;
DROP PROCEDURE IF EXISTS `smartmeal`.`sp_User_GetById`;
DROP PROCEDURE IF EXISTS `smartmeal`.`sp_User_GetByUsername`;
DROP PROCEDURE IF EXISTS `smartmeal`.`sp_User_Insert`;
DROP PROCEDURE IF EXISTS `smartmeal`.`sp_User_UpdateStatus`;
DROP FUNCTION IF EXISTS `smartmeal`.`fx_Table_CheckExists`;
CREATE TABLE `tbl_log` (
  `id` int NOT NULL AUTO_INCREMENT,
  `createtime` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `content` varchar(500) DEFAULT NULL,
  `typeId` int NOT NULL,
  `username` varchar(32) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=72 DEFAULT CHARSET=utf8;
CREATE TABLE `tbl_logtype` (
  `id` int NOT NULL AUTO_INCREMENT,
  `logType` varchar(255) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8;
CREATE TABLE `tbl_order` (
  `id` int NOT NULL AUTO_INCREMENT,
  `tableId` int NOT NULL,
  `statusId` int NOT NULL DEFAULT '1',
  `startTime` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `endTime` datetime NOT NULL DEFAULT '1900-01-01 00:00:00',
  `customerName` varchar(255) DEFAULT NULL,
  `customerContact` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;
CREATE TABLE `tbl_orderstatus` (
  `id` int NOT NULL AUTO_INCREMENT,
  `statusName` varchar(255) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;
CREATE TABLE `tbl_product` (
  `id` int NOT NULL AUTO_INCREMENT,
  `productName` varchar(255) NOT NULL,
  `productPrice` int NOT NULL DEFAULT '0',
  `isActive` int NOT NULL DEFAULT '1',
  `productLineId` int NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;
CREATE TABLE `tbl_productline` (
  `id` int NOT NULL AUTO_INCREMENT,
  `productLineName` varchar(255) NOT NULL,
  `parentId` int NOT NULL DEFAULT '0',
  `isActive` int NOT NULL DEFAULT '1',
  `levelCode` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8;
CREATE TABLE `tbl_roletype` (
  `id` int NOT NULL AUTO_INCREMENT,
  `roleName` varchar(255) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8;
CREATE TABLE `tbl_table` (
  `id` int NOT NULL AUTO_INCREMENT,
  `tableName` varchar(255) NOT NULL,
  `isActive` bit(1) NOT NULL DEFAULT b'1',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8;
CREATE TABLE `tbl_user` (
  `id` int NOT NULL AUTO_INCREMENT,
  `username` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `password` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `fullname` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `birthYear` int NOT NULL,
  `sex` int NOT NULL,
  `roleId` int NOT NULL,
  `isActive` int NOT NULL DEFAULT '1',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_Order_Insert`(IN pTableId INT, IN pStatusId INT, IN pStartTime DATETIME, IN pEndTime DATETIME, IN pCustomerName VARCHAR(50), IN pCustomerContact VARCHAR(20), IN pCreatorId INT)
BEGIN
	INSERT INTO tbl_order (tableId, statusId, startTime, endTime, customerName, customerContact, creatorId) VALUES (pTableId, pStatusId, pStartTime, pEndTime, pCustomerName, pCustomerContact, pCreatorId);
END;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_Order_UpdateStatus`(IN pTableId INT, IN pStatusId INT)
BEGIN
	UPDATE tbl_order o SET o.statusId = pStatusId WHERE o.tableId = pTableId;
END;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_ProductLine_Get`(IN pId INT, IN pName VARCHAR(50), IN pParentId INT, IN pIsActive INT)
BEGIN
	SELECT pl.id AS Id, pl.productLineName AS Name, pl.parentId AS parentId, pl.isActive AS IsActive
	FROM tbl_productline pl
	WHERE (pId = 0 OR pl.id = pId)
		AND (pName IS NULL OR pl.productLineName LIKE CONCAT(pName, "%"))
		AND (pParentId = -1 OR pl.parentId = pParentId)
		AND (pIsActive = -1 OR pl.isActive = pIsActive)
	ORDER BY pl.productLineName ASC;
END;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_Product_ChangeStatus`(IN pId INT, IN pStatusId INT, IN pUsername VARCHAR(32))
BEGIN
	UPDATE tbl_product p SET p.isActive = pStatusId WHERE p.id = pId;
	INSERT INTO tbl_log (content, typeId, username) VALUES (CONCAT("Cập nhật trạng thái sản phẩm (id: ", pId, ") sang ", pStatusId), 6, pUsername);
END;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_Product_Insert`(IN pName VARCHAR(50), IN pProductLineId INT, IN pPrice INT)
BEGIN
	DECLARE productId INT;
	INSERT INTO tbl_product (productName, productLineId, productPrice) VALUES (pName, pProductLineId, pPrice);
	SET productId = LAST_INSERT_ID();
	INSERT INTO tbl_log(content, typeId) VALUES (CONCAT("Thêm sản phẩm mới (", productId, ", ", pName, ", ", pProductLineId, ", ", pPrice, ")"), 6);
	SELECT productId;
END;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_Product_Search`(IN pId INT, IN pName VARCHAR(50), IN pStartPrice INT, IN pEndPrice INT, pIsActive INT)
BEGIN
	SELECT p.id AS Id,
		p.productName AS `Name`,
		p.productLineId AS ProductLineId,
		p.productPrice AS Price,
		p.isActive AS IsActive
	FROM tbl_product p
	WHERE (pId = 0 OR p.id = pId)
		AND ((pName IS NULL) OR (p.productName LIKE CONCAT(pName, '%')))
		AND (pStartPrice = -1 OR pStartPrice <= p.productPrice)
		AND (pEndPrice = -1 OR pEndPrice >= p.productPrice)
		AND (pIsActive = -1 OR pIsActive = p.isActive);
END;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_Table_ChangeName`(IN pId INT, IN pTableName VARCHAR(50))
BEGIN
	UPDATE tlb_table SET tableName = pTableName WHERE id = pId;
	INSERT INTO tbl_log (content, typeId) VALUES (CONCAT("Đổi tên bàn (id: ", pId, "): ", pTableName),  5);
END;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_Table_GetAllActive`()
BEGIN
	SELECT t.id AS ID,
		t.tableName AS TableName,
		1 AS IsActive,
		IF ((o.id > 0), o.statusId, 0) AS `Status`
	FROM tbl_table t
	LEFT JOIN tbl_order o ON o.id = (SELECT od.id FROM tbl_order od WHERE od.tableId = t.id AND od.statusId <> 3 AND od.statusId <> 4 ORDER BY od.id DESC LIMIT 1)
	WHERE t.isActive = 1;
END;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_Table_GetById`(IN pId INT)
BEGIN
	SELECT t.id AS ID,
		t.tableName AS TableName,
		t.isActive AS IsActive,
		IF (t.isActive = 1, (IF ((o.id > 0), o.statusId, 0)), -1) AS `Status`
	FROM tbl_table t
	LEFT JOIN tbl_order o ON o.id = (SELECT od.id FROM tbl_order od WHERE od.tableId = t.id AND od.statusId <> 3 AND od.statusId <> 4 ORDER BY od.id DESC LIMIT 1)
	WHERE t.id = pId;
END;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_Table_Insert`(IN pTableName VARCHAR(50))
BEGIN
	IF ((fx_Table_CheckExists(pTableName)) = 1) THEN
		SELECT "Tên bàn đã tồn tại.";
	ELSE 
		BEGIN
			DECLARE tableID INT;
			INSERT INTO tbl_table (tableName) VALUES (pTableName);
			SET tableID = LAST_INSERT_ID();
			INSERT INTO tbl_log (content, typeId) VALUES (CONCAT("Thêm bàn '", pTableName, "' (id: ", tableID, ")"), 5);
			SELECT tableID;
		END;
	END IF;
END;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_User_CheckLogin`(IN pUsername VARCHAR(50), IN pPassword VARCHAR(50))
BEGIN
	SELECT (EXISTS (SELECT u.id FROM tbl_user u WHERE u.username = pUserName AND u.`password` = pPassword)) AS IsChecked;
	INSERT INTO tbl_log (content, typeId, username) VALUE (CONCAT("Đăng nhập (", pUsername, ", ", pPassword, ")"), 1, pUsername);
END;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_User_GetById`(IN pId INT)
BEGIN
	SELECT u.id AS Id, u.username As Username, u.fullname AS Fullname, u.birthYear AS BirthYear, u.sex As Sex, u.roleid AS RoleId FROM tbl_user u WHERE u.id = pId;
END;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_User_GetByUsername`(IN pUsername VARCHAR(50))
BEGIN
	SELECT u.id AS Id, u.username As Username, u.fullname AS Fullname, u.birthYear AS BirthYear, u.sex As Sex, u.roleid AS RoleId FROM tbl_user u WHERE u.username = pUsername;
END;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_User_Insert`(IN pUsername VARCHAR(32), IN pPassword VARCHAR(32), IN pFullname VARCHAR(50), IN pBirthYear INT, IN pSex INT, IN pRoleId INT)
BEGIN
	INSERT INTO tbl_user (username, `password`, fullname, birthYear, sex, roleId) VALUES (pUsername, pPassword, pFullname, pBirthYear, pSex, pRoleId);
	INSERT INTO tbl_log (content, typeId) VALUES (CONCAT("Đăng ký (", pUsername, ", ", pPassword, ", ", pFullname, ", ", pBirthYear, ", ", pSex, ", ", pRoleId, ")"), 1);
END;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_User_UpdateStatus`(IN pId INT, IN pStatus INT)
BEGIN
	UPDATE tbl_user u SET isActive = pStatus WHERE u.id = pId;
	IF (pStatusId = 1) THEN
		INSERT INTO tbl_log (content, typeId) VALUES(CONCAT("Kích hoạt tài khoản ",  pId, " - ", (SELECT u.username FROM tbl_user u WHERE u.id = pId)), 1);
		ELSE 
			INSERT INTO tbl_log (content, typeId) VALUES(CONCAT("Huỷ kích hoạt tài khoản ",  pId, " - ", (SELECT u.username FROM tbl_user u WHERE u.id = pId)), 1);
		END IF;
END;
CREATE DEFINER=`root`@`localhost` FUNCTION `fx_Table_CheckExists`(pTableName VARCHAR(50)) RETURNS int
    DETERMINISTIC
BEGIN
	RETURN IF (EXISTS (SELECT id FROM tbl_table WHERE tableName = pTableName), 1, 0);
END;
BEGIN;
LOCK TABLES `smartmeal`.`tbl_log` WRITE;
DELETE FROM `smartmeal`.`tbl_log`;
INSERT INTO `smartmeal`.`tbl_log` (`id`,`createtime`,`content`,`typeId`,`username`) VALUES (1, '2021-04-25 21:07:07', 'Login (admin, 0192023a7bbd73250516f069df18b500)', 1, NULL),(2, '2021-04-25 21:07:09', 'Login (admin, 0192023a7bbd73250516f069df18b500)', 1, NULL),(3, '2021-04-25 21:07:08', 'Login (admin, 0192023a7bbd73250516f069df18b50)', 1, NULL),(4, '2021-04-25 21:07:08', 'Login (admin, 0192023a7bbd73250516f069df18b500)', 1, NULL),(5, '2021-04-25 21:07:10', 'Login (admin, 0192023a7bbd73250516f069df18b500)', 1, NULL),(6, '2021-04-25 21:07:10', 'Login (admin, 0192023a7bbd73250516f069df18b500)', 1, NULL),(7, '2021-04-25 21:07:11', 'Login (admin, 0192023a7bbd73250516f069df18b500)', 1, NULL),(8, '2021-04-25 21:07:11', 'Login (admin, 0192023a7bbd73250516f069df18b500)', 1, NULL),(9, '2021-04-25 21:07:11', 'Login (admin, 0192023a7bbd73250516f069df18b500)', 1, NULL),(10, '2021-04-25 21:07:13', 'Login (admin, 0192023a7bbd73250516f069df18b500)', 1, NULL),(11, '2021-04-25 21:07:12', 'Login (admin, 0192023a7bbd73250516f069df18b500)', 1, NULL),(12, '2021-04-25 21:07:14', 'Login (admin, 0192023a7bbd73250516f069df18b500)', 1, NULL),(13, '2021-04-25 21:07:12', 'Login (admin, 0192023a7bbd73250516f069df18b500)', 1, NULL),(14, '2021-04-25 21:07:14', 'Login (admin, 0192023a7bbd73250516f069df18b500)', 1, NULL),(15, '2021-04-25 21:07:15', 'Login (admin, 0192023a7bbd73250516f069df18b500)', 1, NULL),(16, '2021-04-25 21:07:16', 'Login (admin, 0192023a7bbd73250516f069df18b500)', 1, NULL),(17, '2021-04-25 21:07:16', 'Login (admin, 0192023a7bbd73250516f069df18b500)', 1, NULL),(18, '2021-04-25 21:07:17', 'Login (admin, 0192023a7bbd73250516f069df18b500)', 1, NULL),(19, '2021-04-25 21:07:17', 'Login (admin, 0192023a7bbd73250516f069df18b500)', 1, NULL),(20, '2021-04-25 21:07:17', 'Login (admin, 0192023a7bbd73250516f069df18b500)', 1, NULL),(21, '2021-04-25 21:07:18', 'Login (admin, 0192023a7bbd73250516f069df18b500)', 1, NULL),(22, '2021-04-25 21:07:18', 'Login (admin, 0192023a7bbd73250516f069df18b500)', 1, NULL),(23, '2021-04-25 21:07:19', 'Login (admin, 0192023a7bbd73250516f069df18b500)', 1, NULL),(24, '2021-04-25 21:07:19', 'Login (admin, 0192023a7bbd73250516f069df18b500)', 1, NULL),(25, '2021-04-25 21:07:20', 'Login (admin, 0192023a7bbd73250516f069df18b500)', 1, NULL),(26, '2021-04-25 21:07:20', 'Login (admin, 0192023a7bbd73250516f069df18b500)', 1, NULL),(27, '2021-04-25 21:07:20', 'Login (admin, 0192023a7bbd73250516f069df18b500)', 1, NULL),(28, '2021-04-25 21:07:21', 'Login (admin, 0192023a7bbd73250516f069df18b500)', 1, NULL),(29, '2021-04-25 21:07:21', 'Login (admin, 0192023a7bbd73250516f069df18b500)', 1, NULL),(30, '2021-04-25 21:07:21', 'Login (admin, 0192023a7bbd73250516f069df18b500)', 1, NULL),(31, '2021-04-25 21:07:22', 'Login (admin, 0192023a7bbd73250516f069df18b500)', 1, NULL),(32, '2021-04-25 21:07:24', 'Login (admin, 0192023a7bbd73250516f069df18b500)', 1, NULL),(33, '2021-04-25 21:07:23', 'Login (admin, 0192023a7bbd73250516f069df18b500)', 1, NULL),(34, '2021-04-25 21:07:23', 'Login (admin, 0192023a7bbd73250516f069df18b500)', 1, NULL),(35, '2021-04-25 21:07:25', 'Login (admin, 0192023a7bbd73250516f069df18b500)', 1, NULL),(36, '2021-04-25 21:07:26', 'Login (admin, 0192023a7bbd73250516f069df18b50)', 1, NULL),(37, '2021-04-25 21:07:27', 'Login (admin, 0192023a7bbd73250516f069df18b500)', 1, NULL),(38, '2021-04-25 21:07:27', 'Login (admin, 0192023a7bbd73250516f069df18b500)', 1, NULL),(39, '2021-04-25 21:07:28', 'Login (admin, 0192023a7bbd73250516f069df18b500)', 1, NULL),(40, '2021-04-25 21:07:28', 'Login (admin, 0192023a7bbd73250516f069df18b500)', 1, NULL),(41, '2021-04-25 21:07:28', 'Login (admin, 0192023a7bbd73250516f069df18b500)', 1, NULL),(42, '2021-04-25 21:07:29', 'Login (admin, 0192023a7bbd73250516f069df18b500)', 1, NULL),(43, '2021-04-25 21:07:29', 'Login (admin, 0192023a7bbd73250516f069df18b500)', 1, NULL),(44, '2021-04-25 21:07:31', 'Login (admin, 0192023a7bbd73250516f069df18b500)', 1, NULL),(45, '2021-04-25 21:07:30', 'Login (admin, 0192023a7bbd73250516f069df18b500)', 1, NULL),(46, '2021-04-25 21:07:32', 'Login (admin, 0192023a7bbd73250516f069df18b500)', 1, NULL),(47, '2021-04-25 21:07:33', 'Login (admin, 0192023a7bbd73250516f069df18b500)', 1, NULL),(48, '2021-04-25 21:07:34', 'Login (admin, 0192023a7bbd73250516f069df18b500)', 1, NULL),(49, '2021-04-25 21:07:34', 'Đăng nhập (admin, 0192023a7bbd73250516f069df18b500)', 1, NULL),(50, '2021-04-25 21:07:35', 'Đăng ký (user1, 6ad14ba9986e3615423dfca256d04e3f, User 1, 2000, 1, 2)', 1, NULL),(51, '2021-04-25 21:07:35', 'Thêm bàn \'Bàn 9\'(id: 9)', 5, NULL),(52, '2021-04-25 21:07:36', 'Thêm bàn \'Bàn 10\'(id: 10)', 5, NULL),(53, '2021-04-25 21:07:37', 'Thêm bàn \'Bàn 11\' (id: 11)', 5, NULL),(54, '2021-04-25 21:07:37', 'Đăng nhập (admin, 0192023a7bbd73250516f069df18b500)', 1, NULL),(55, '2021-04-25 21:07:38', 'Đăng nhập (admin, 0192023a7bbd73250516f069df18b500)', 1, NULL),(56, '2021-04-25 21:07:38', 'Đăng nhập (admin, 0192023a7bbd73250516f069df18b500)', 1, NULL),(57, '2021-04-25 21:07:39', 'Đăng nhập (admin, 0192023a7bbd73250516f069df18b500)', 1, NULL),(58, '2021-04-25 21:07:39', 'Đăng nhập (admin, 0192023a7bbd73250516f069df18b500)', 1, NULL),(59, '2021-04-25 21:07:40', 'Đăng nhập (admin, 0192023a7bbd73250516f069df18b500)', 1, NULL),(62, '2021-04-25 21:07:41', 'Thêm sản phẩm mới (3, Coca-cola, 4, 10000)', 6, NULL),(63, '2021-04-25 21:24:39', 'Đăng nhập (admin, 0192023a7bbd73250516f069df18b500)', 1, NULL),(64, '2021-04-25 23:51:41', 'Đăng nhập (admin, 0192023a7bbd73250516f069df18b500)', 1, NULL),(65, '2021-04-25 23:53:34', 'Đăng nhập (admin, 0192023a7bbd73250516f069df18b500)', 1, NULL),(66, '2021-04-26 00:21:22', 'Đăng nhập (admin, 0192023a7bbd73250516f069df18b500)', 1, 'admin'),(67, '2021-04-26 00:39:33', 'Đăng nhập (admin, 0192023a7bbd73250516f069df18b500)', 1, 'admin'),(68, '2021-04-26 00:47:01', 'Đăng nhập (admin, 0192023a7bbd73250516f069df18b500)', 1, 'admin'),(69, '2021-04-26 00:47:58', 'Đăng nhập (admin, 0192023a7bbd73250516f069df18b500)', 1, 'admin'),(70, '2021-04-26 00:49:17', 'Cập nhật trạng thái sản phẩm (id: 3) sang 1', 6, 'admin'),(71, '2021-04-26 00:53:52', 'Cập nhật trạng thái sản phẩm (id: 3) sang 1', 6, 'admin');
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `smartmeal`.`tbl_logtype` WRITE;
DELETE FROM `smartmeal`.`tbl_logtype`;
INSERT INTO `smartmeal`.`tbl_logtype` (`id`,`logType`) VALUES (1, 'Tài khoản'),(2, 'Đặt bàn'),(3, 'Gọi đồ'),(4, 'Sản phẩm'),(5, 'Quản lý bàn'),(6, 'Quản lý sản phẩm');
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `smartmeal`.`tbl_order` WRITE;
DELETE FROM `smartmeal`.`tbl_order`;
INSERT INTO `smartmeal`.`tbl_order` (`id`,`tableId`,`statusId`,`startTime`,`endTime`,`customerName`,`customerContact`) VALUES (1, 1, 1, '2021-04-24 23:18:27', '1900-01-01 00:00:00', 'Nguyễn Mạnh Hùng', '0388855455');
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `smartmeal`.`tbl_orderstatus` WRITE;
DELETE FROM `smartmeal`.`tbl_orderstatus`;
INSERT INTO `smartmeal`.`tbl_orderstatus` (`id`,`statusName`) VALUES (1, 'Đang có khách'),(2, 'Đã đặt'),(3, 'Đã thanh toán'),(4, 'Đã huỷ');
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `smartmeal`.`tbl_product` WRITE;
DELETE FROM `smartmeal`.`tbl_product`;
INSERT INTO `smartmeal`.`tbl_product` (`id`,`productName`,`productPrice`,`isActive`,`productLineId`) VALUES (3, 'Coca-cola', 10000, 1, 4);
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `smartmeal`.`tbl_productline` WRITE;
DELETE FROM `smartmeal`.`tbl_productline`;
INSERT INTO `smartmeal`.`tbl_productline` (`id`,`productLineName`,`parentId`,`isActive`,`levelCode`) VALUES (1, 'Món tráng miệng', 0, 1, '001'),(2, 'Đồ uống', 0, 1, '002'),(3, 'Bia', 2, 1, '002003'),(4, 'Nước ngọt', 2, 1, '002004'),(5, 'Món khai vị', 0, 1, '005');
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `smartmeal`.`tbl_roletype` WRITE;
DELETE FROM `smartmeal`.`tbl_roletype`;
INSERT INTO `smartmeal`.`tbl_roletype` (`id`,`roleName`) VALUES (1, 'Admin'),(2, 'Quản lý'),(3, 'Thu ngân'),(4, 'Phục vụ'),(5, 'Đầu bếp');
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `smartmeal`.`tbl_table` WRITE;
DELETE FROM `smartmeal`.`tbl_table`;
INSERT INTO `smartmeal`.`tbl_table` (`id`,`tableName`,`isActive`) VALUES (1, 'Bàn 1', b'1'),(2, 'Bàn 2', b'1'),(3, 'Bàn 3', b'1'),(4, 'Bàn 4', b'1'),(5, 'Bàn 5', b'1'),(6, 'Bàn 6', b'1'),(7, 'Bàn 7', b'0'),(8, 'Bàn 8', b'1'),(9, 'Bàn 9', b'1'),(10, 'Bàn 10', b'1'),(11, 'Bàn 11', b'1');
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `smartmeal`.`tbl_user` WRITE;
DELETE FROM `smartmeal`.`tbl_user`;
INSERT INTO `smartmeal`.`tbl_user` (`id`,`username`,`password`,`fullname`,`birthYear`,`sex`,`roleId`,`isActive`) VALUES (1, 'admin', '0192023a7bbd73250516f069df18b500', 'Admin', 2000, 1, 1, 1),(2, 'manager', '0795151defba7a4b5dfa89170de46277', 'Manager', 2000, 1, 2, 1),(3, 'user1', '6ad14ba9986e3615423dfca256d04e3f', 'User 1', 2000, 1, 3, 1);
UNLOCK TABLES;
COMMIT;
