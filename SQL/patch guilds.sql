/*
Navicat MySQL Data Transfer

Source Server         : localhost_3306
Source Server Version : 50617
Source Host           : localhost:3306
Source Database       : symbioz

Target Server Type    : MYSQL
Target Server Version : 50617
File Encoding         : 65001

Date: 2016-05-01 02:00:00
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for guilds
-- ----------------------------
DROP TABLE IF EXISTS `guilds`;
CREATE TABLE `guilds` (
  `Id` int(250) NOT NULL DEFAULT '0',
  `Name` varchar(250) DEFAULT NULL,
  `SymbolShape` int(250) DEFAULT NULL,
  `SymbolColor` int(250) DEFAULT NULL,
  `BackgroundShape` tinyint(250) DEFAULT NULL,
  `BackgroundColor` int(250) DEFAULT NULL,
  `Level` int(250) DEFAULT NULL,
  `Experience` bigint(255) DEFAULT NULL,
  `MaxTaxCollectors` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
