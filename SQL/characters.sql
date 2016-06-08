/*
Navicat MySQL Data Transfer

Source Server         : localhost_3306
Source Server Version : 50520
Source Host           : localhost:3306
Source Database       : symbioz

Target Server Type    : MYSQL
Target Server Version : 50520
File Encoding         : 65001

Date: 2016-06-08 06:30:05
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
  `Succes` text,
  `CurrentLifePoint` int(11) DEFAULT NULL,
  `LastConnection` int(11) DEFAULT NULL,
  `WarnOnFriendConnection` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of characters
-- ----------------------------
INSERT INTO `characters` VALUES ('2', 'Nojoga', '6', '{1|80,2124|1=33542780,1=49910315,1=67108833,1=70018928,1=98447879,1=100663296,1=118365580,1=147847718,1=151992911,1=173036546|140}', '{1|80,2124|1=33542780,1=49910315,1=67108833,1=70018928,1=98447879|140}', '11', '8', 'False', '84674561', '468', '2', '332', '29183', '0', '0', '0', '0', '0', '0', '50', '11', '0', '', '', '0', '0', '1,97,98', '-1', '0', '154010883', '300', '300', '300', '0', '0', 'False', '10000', '0', '1', '', '98', '1465366763', '0');
INSERT INTO `characters` VALUES ('3', 'Nety', '1', '{1|80,2124|1=33542780,1=49910315,1=67108833,1=70018928,1=98447879,1=100663296,1=119563562,1=134654486,1=151992911,1=173036546|140}', '{1|80,2124|1=33542780,1=49910315,1=67108833,1=70018928,1=98447879|140}', '200', '8', 'False', '84674561', '509', '2', '3086', '7407232000', '0', '0', '0', '0', '0', '0', '0', '50', '0', '', '13,14,15', '0', '0', '1,22,97,98,6', '-1', '0', '154010883,154010371', '300', '300', '300', '0', '0', 'False', '10000', '0', '1', '', '1134', '1465366298', '0');
INSERT INTO `characters` VALUES ('4', 'Gikima-xe', '6', '{1|80,2124|1=33542780,1=49910315,1=67108833,1=70018928,1=98447879|140}', '{1|80,2124|1=33542780,1=49910315,1=67108833,1=70018928,1=98447879|140}', '1', '8', 'False', '84674561', '357', '6', '0', '1', '0', '0', '0', '0', '0', '0', '0', '1', '0', '', '', '0', '0', '1', '-1', '0', '154010883', '300', '300', '300', '0', '0', 'False', '10000', '0', '1', '', '48', '1465365202', '0');
INSERT INTO `characters` VALUES ('5', 'Jiqy', '6', '{1|110,2172|1=33542780,1=50307866,1=60141352,1=72773666,1=100657218|145}', '{1|110,2172|1=33542780,1=50307866,1=60141352,1=72773666,1=100657218|145}', '1', '11', 'False', '154010883', '375', '6', '0', '1', '0', '0', '0', '0', '0', '0', '0', '1', '0', '', '', '0', '0', '1', '-1', '0', '154010883', '300', '300', '300', '0', '0', 'False', '10000', '0', '1', '', '46', '1465366305', '0');
