/*
Navicat MySQL Data Transfer

Source Server         : localhost_3306
Source Server Version : 50617
Source Host           : localhost:3306
Source Database       : symbioz

Target Server Type    : MYSQL
Target Server Version : 50617
File Encoding         : 65001

Date: 2016-05-01 01:49:33
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for alliances
-- ----------------------------
DROP TABLE IF EXISTS `alliances`;
CREATE TABLE `alliances` (
  `id` int(11) NOT NULL,
  `name` varchar(255) DEFAULT NULL,
  `tag` varchar(255) DEFAULT NULL,
  `symbolColor` int(11) DEFAULT NULL,
  `symbolShape` int(11) DEFAULT NULL,
  `backgroundColor` int(11) DEFAULT NULL,
  `backgroundShape` int(11) DEFAULT NULL,
  `leaderguildid` int(11) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for guildsalliances
-- ----------------------------
DROP TABLE IF EXISTS `guildsalliances`;
CREATE TABLE `guildsalliances` (
  `GuildId` int(11) NOT NULL,
  `AllianceId` int(11) DEFAULT NULL,
  PRIMARY KEY (`GuildId`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;