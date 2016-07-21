/*
Navicat MySQL Data Transfer

Source Server         : localhost_3306
Source Server Version : 50520
Source Host           : localhost:3306
Source Database       : symbioz

Target Server Type    : MYSQL
Target Server Version : 50520
File Encoding         : 65001

Date: 2016-07-21 08:48:25
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `challenges`
-- ----------------------------
DROP TABLE IF EXISTS `challenges`;
CREATE TABLE `challenges` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ChallengeId` int(11) NOT NULL,
  `ChallengeName` varchar(255) NOT NULL,
  `ChallengeXpBonus` int(11) NOT NULL,
  `ChallengeDropBonus` int(11) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=35 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of challenges
-- ----------------------------
INSERT INTO `challenges` VALUES ('1', '1', 'Zombie', '50', '50');
INSERT INTO `challenges` VALUES ('2', '2', 'Statue', '25', '25');
INSERT INTO `challenges` VALUES ('3', '3', 'Désigné volontaire', '30', '30');
INSERT INTO `challenges` VALUES ('4', '4', 'Sursis', '50', '50');
INSERT INTO `challenges` VALUES ('5', '5', 'Econome', '80', '80');
INSERT INTO `challenges` VALUES ('6', '6', 'Versatile', '30', '30');
INSERT INTO `challenges` VALUES ('7', '7', 'Jardinier', '15', '15');
INSERT INTO `challenges` VALUES ('8', '8', 'Nomade', '35', '35');
INSERT INTO `challenges` VALUES ('9', '9', 'Barbare', '45', '25');
INSERT INTO `challenges` VALUES ('10', '10', 'Cruel', '80', '80');
INSERT INTO `challenges` VALUES ('11', '11', 'Mystique', '50', '50');
INSERT INTO `challenges` VALUES ('12', '12', 'Fossoyeur', '35', '35');
INSERT INTO `challenges` VALUES ('13', '13', 'Casino Royal', '45', '45');
INSERT INTO `challenges` VALUES ('14', '14', 'Araknophile', '45', '25');
INSERT INTO `challenges` VALUES ('15', '17', 'Intouchable', '100', '100');
INSERT INTO `challenges` VALUES ('16', '18', 'Incurable', '35', '35');
INSERT INTO `challenges` VALUES ('17', '19', 'Mains propres', '120', '120');
INSERT INTO `challenges` VALUES ('18', '20', 'Elementaire', '50', '50');
INSERT INTO `challenges` VALUES ('19', '21', 'Circulez', '40', '40');
INSERT INTO `challenges` VALUES ('20', '22', 'Le temp qui court', '45', '45');
INSERT INTO `challenges` VALUES ('21', '23', 'Perdu de vue', '35', '35');
INSERT INTO `challenges` VALUES ('22', '25', 'Ordonné', '80', '80');
INSERT INTO `challenges` VALUES ('23', '28', 'Ni pioutes ni soumises', '80', '80');
INSERT INTO `challenges` VALUES ('24', '29', 'Ni pious ni soumis', '80', '80');
INSERT INTO `challenges` VALUES ('25', '30', 'Les petits d\'abord', '80', '80');
INSERT INTO `challenges` VALUES ('26', '31', 'Focus', '50', '50');
INSERT INTO `challenges` VALUES ('27', '32', 'Elitiste', '50', '50');
INSERT INTO `challenges` VALUES ('28', '33', 'Survivant', '70', '70');
INSERT INTO `challenges` VALUES ('29', '34', 'Imprevisible', '100', '100');
INSERT INTO `challenges` VALUES ('30', '35', 'Tueurs à gages', '100', '100');
INSERT INTO `challenges` VALUES ('31', '36', 'Hardi', '75', '75');
INSERT INTO `challenges` VALUES ('32', '37', 'Collant', '75', '75');
INSERT INTO `challenges` VALUES ('33', '38', 'Blitzkrieg', '80', '80');
INSERT INTO `challenges` VALUES ('34', '41', 'Pétulant', '50', '50');
