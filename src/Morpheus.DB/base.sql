-- --------------------------------------------------------
-- Servidor:                     127.0.0.1
-- Versão do servidor:           5.7.17-log - MySQL Community Server (GPL)
-- OS do Servidor:               Win64
-- HeidiSQL Versão:              9.4.0.5125
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- Copiando estrutura para tabela marias.users
DROP TABLE IF EXISTS `users`;
CREATE TABLE IF NOT EXISTS `users` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(500) NOT NULL,
  `Email` varchar(500) NOT NULL,
  `UId` varchar(500) NOT NULL,
  `PhotoUrl` varchar(500) NOT NULL,
  `DateOfBirth` date DEFAULT NULL,
  `Address` varchar(800) NOT NULL,
  `Address2` varchar(800) DEFAULT NULL,
  `City` varchar(500) NOT NULL,
  `State` varchar(200) NOT NULL,
  `Country` varchar(200) NOT NULL,
  `ZipCode` varchar(20) NOT NULL,
  `IsDeleted` bit(1) NOT NULL DEFAULT b'0',
  `LastAccessDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `TokenExpirationDate` datetime NOT NULL,
  `AddedDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `ModifiedDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;


/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
