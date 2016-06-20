/*
Navicat MySQL Data Transfer

Source Server         : localhost_3306
Source Server Version : 50520
Source Host           : localhost:3306
Source Database       : symbioz

Target Server Type    : MYSQL
Target Server Version : 50520
File Encoding         : 65001

Date: 2016-06-19 07:09:50
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `charactersmerchants`
-- ----------------------------
DROP TABLE IF EXISTS `charactersmerchants`;
CREATE TABLE `charactersmerchants` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `CharacterId` int(11) DEFAULT NULL,
  `ItemUID` int(11) DEFAULT NULL,
  `Price` int(11) DEFAULT NULL,
  `Quantity` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of charactersmerchants
-- ----------------------------
INSERT INTO `charactersmerchants` VALUES ('2', '1', '125', '1', '2');
INSERT INTO `charactersmerchants` VALUES ('3', '1', '126', '1000', '1');
