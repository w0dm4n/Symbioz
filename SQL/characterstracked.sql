/*
Navicat MySQL Data Transfer

Source Server         : localhost_3306
Source Server Version : 50520
Source Host           : localhost:3306
Source Database       : symbioz

Target Server Type    : MYSQL
Target Server Version : 50520
File Encoding         : 65001

Date: 2016-06-14 21:00:05
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `characterstracked`
-- ----------------------------
DROP TABLE IF EXISTS `characterstracked`;
CREATE TABLE `characterstracked` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `TrackedCharacterId` int(11) NOT NULL,
  `ItemUID` int(11) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=20 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of characterstracked
-- ----------------------------
