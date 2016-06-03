/*
Navicat MySQL Data Transfer

Source Server         : localhost_3306
Source Server Version : 50520
Source Host           : localhost:3306
Source Database       : symbioz

Target Server Type    : MYSQL
Target Server Version : 50520
File Encoding         : 65001

Date: 2016-06-03 02:44:54
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `characters`
-- ----------------------------
DROP TABLE IF EXISTS `characters`;
CREATE TABLE `characters` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) DEFAULT NULL,
  `AccountId` int(11) DEFAULT NULL,
  `Look` varchar(255) DEFAULT NULL,
  `OldLook` varchar(255) DEFAULT NULL,
  `Level` varchar(255) DEFAULT NULL,
  `Breed` varchar(255) DEFAULT NULL,
  `Sex` varchar(255) DEFAULT NULL,
  `MapId` int(11) DEFAULT NULL,
  `CellId` int(11) DEFAULT NULL,
  `Direction` int(255) DEFAULT NULL,
  `Kamas` int(255) DEFAULT NULL,
  `Exp` bigint(255) DEFAULT NULL,
  `TitleId` int(11) DEFAULT NULL,
  `OrnamentId` int(11) DEFAULT NULL,
  `AlignmentSide` int(11) DEFAULT NULL,
  `AlignmentValue` int(11) DEFAULT NULL,
  `AlignmentGrade` int(11) DEFAULT NULL,
  `CharacterPower` int(11) NOT NULL,
  `StatsPoints` int(11) DEFAULT NULL,
  `SpellPoints` int(11) DEFAULT NULL,
  `Honor` int(11) DEFAULT NULL,
  `KnownTiles` varchar(9999) DEFAULT NULL,
  `KnownOrnaments` varchar(9999) DEFAULT NULL,
  `ActiveTitle` int(11) DEFAULT NULL,
  `ActiveOrnament` int(11) DEFAULT NULL,
  `KnownEmotes` varchar(9999) DEFAULT NULL,
  `SpawnPointMapId` int(11) DEFAULT NULL,
  `EquipedSkitterId` int(11) DEFAULT NULL,
  `KnownTips` varchar(9999) DEFAULT NULL,
  `ActualRank` int(11) DEFAULT NULL,
  `BestDailyRank` int(11) DEFAULT NULL,
  `MaxRank` int(11) DEFAULT NULL,
  `ArenaVictoryCount` int(11) DEFAULT NULL,
  `ArenaFightCount` int(11) DEFAULT NULL,
  `PvpEnable` varchar(11) DEFAULT NULL,
  `Energy` int(11) DEFAULT '10000',
  `DeathCount` int(11) DEFAULT '0',
  `DeathMaxLevel` int(11) DEFAULT '1',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of characters
-- ----------------------------
INSERT INTO `characters` VALUES ('1', 'Taki', '1', '{1|80,2124|1=28414802,1=36017610,1=51258685,1=69223306,1=87478761|140}', '{1|80,2124|1=28414802,1=36017610,1=51258685,1=69223306,1=87478761|140}', '1', '8', 'False', '154010883', '304', '7', '0', '1', '0', '0', '0', '0', '0', '0', '5', '1', '0', '', '', '0', '0', '1', '-1', '0', '154010883', '300', '300', '300', '0', '0', 'False', '10000', '3', '1');
