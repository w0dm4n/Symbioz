/*
Navicat MySQL Data Transfer

Source Server         : localhost_3306
Source Server Version : 50520
Source Host           : localhost:3306
Source Database       : symbioz

Target Server Type    : MYSQL
Target Server Version : 50520
File Encoding         : 65001

Date: 2016-06-09 17:07:59
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `charactersfriends`
-- ----------------------------
DROP TABLE IF EXISTS `charactersfriends`;
CREATE TABLE `charactersfriends` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `AccountId` int(11) DEFAULT NULL,
  `FriendAccountId` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of charactersfriends
-- ----------------------------
