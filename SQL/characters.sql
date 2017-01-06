/*
Navicat MySQL Data Transfer

Source Server         : localhost_3306
Source Server Version : 50520
Source Host           : localhost:3306
Source Database       : symbioz

Target Server Type    : MYSQL
Target Server Version : 50520
File Encoding         : 65001

Date: 2016-06-19 07:09:35
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
  `Look` text,
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
  `Succes` text,
  `CurrentLifePoint` int(11) DEFAULT NULL,
  `LastConnection` int(11) DEFAULT NULL,
  `MoodSmileyId` int(11) DEFAULT NULL,
  `MerchantMode` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of characters
-- ----------------------------
INSERT INTO `characters` VALUES ('1', 'Nety', '1', '{1|80,2124,2521,1428|1=33542780,1=49910315,1=67108833,1=70018928,1=98447879|140|1@0={260|||80}}', '{1|80,2124|1=33542780,1=49910315,1=67108833,1=70018928,1=98447879|140}', '200', '8', 'False', '84674562', '428', '3', '88644766', '7407232000', '0', '0', '1', '0', '1', '0', '3', '2', '0', '', '13,14,15', '0', '0', '1,22', '144419', '0', '154010883', '300', '300', '300', '0', '0', 'False', '10000', '0', '1', '', '3784', '1466319471', '0', '1');
INSERT INTO `characters` VALUES ('2', 'Kyco', '1', '{1|80,2124|1=33542780,1=49910315,1=67108833,1=70018928,1=98447879|140}', '{1|80,2124|1=33542780,1=49910315,1=67108833,1=70018928,1=98447879|140}', '200', '8', 'False', '154010883', '370', '3', '0', '7407232000', '0', '0', '0', '0', '0', '0', '995', '200', '0', '', '13,14,15', '0', '0', '1,22', '-1', '0', '154010883', '300', '300', '300', '0', '0', 'False', '10000', '0', '1', '', '1043', '0', '0', '0');
INSERT INTO `characters` VALUES ('3', 'Foqego', '7', '{1|81,2135,2952,1330,1329|1=15568431,1=5451534,1=16711360,1=1801634,1=16711360|140|1@0={196|||75},1@0={196|||75},1@0={196|||75},1@0={196|||75},1@0={196|||75},1@0={196|||75},1@0={196|||75},1@0={196|||75},1@0={196|||75},1@0={196|||75},1@0={196|||75},1@0={196|||75},1@0={196|||75},1@0={196|||75},1@0={196|||75},1@0={196|||75},1@0={196|||75},1@0={196|||75},1@0={196|||75},1@0={196|||75},1@0={196|||75},1@0={196|||75},1@0={196|||75},1@0={196|||75},1@0={196|||75},1@0={196|||75},1@0={196|||75},1@0={196|||75},1@0={196|||75},1@0={196|||75},1@0={196|||75},1@0={196|||75},1@0={196|||75},1@0={196|||75},1@0={196|||75},1@0={196|||75},1@0={196|||75},1@0={196|||75},1@0={196|||75},1@0={196|||75},1@0={196|||75},1@0={196|||75},1@0={196|||75}}', '{1|11,2020|1=32880991,1=40387616,1=63992838,1=80770054,1=90849542|125}', '1', '1', 'True', '84674563', '371', '6', '0', '1', '0', '0', '0', '0', '0', '0', '0', '1', '0', '', '', '0', '0', '1', '-1', '0', '154010883', '300', '300', '300', '0', '0', 'False', '10000', '0', '1', '', '42', '1466317191', '0', '0');
