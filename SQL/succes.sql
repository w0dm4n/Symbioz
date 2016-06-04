/*
Navicat MySQL Data Transfer

Source Server         : localhost_3306
Source Server Version : 50520
Source Host           : localhost:3306
Source Database       : symbioz

Target Server Type    : MYSQL
Target Server Version : 50520
File Encoding         : 65001

Date: 2016-06-03 22:02:16
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `succes`
-- ----------------------------
DROP TABLE IF EXISTS `succes`;
CREATE TABLE `succes` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `SuccesId` int(11) NOT NULL,
  `SubAreaDiscoveredId` int(11) NOT NULL,
  `KamasRatio` int(11) NOT NULL,
  `XPRatio` int(11) NOT NULL,
  `MonsterId` int(11) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of succes
-- ----------------------------
