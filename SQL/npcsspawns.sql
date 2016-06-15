/*
Navicat MySQL Data Transfer

Source Server         : localhost_3306
Source Server Version : 50520
Source Host           : localhost:3306
Source Database       : symbioz

Target Server Type    : MYSQL
Target Server Version : 50520
File Encoding         : 65001

Date: 2016-06-12 17:15:54
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `npcsspawns`
-- ----------------------------
DROP TABLE IF EXISTS `npcsspawns`;
CREATE TABLE `npcsspawns` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `TemplateId` int(11) DEFAULT NULL,
  `MapId` int(11) DEFAULT NULL,
  `CellId` int(11) DEFAULT NULL,
  `Direction` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=550 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of npcsspawns
-- ----------------------------
INSERT INTO `npcsspawns` VALUES ('1', '585', '143397', '372', '3');
INSERT INTO `npcsspawns` VALUES ('2', '100', '8914959', '343', '3');
INSERT INTO `npcsspawns` VALUES ('3', '816', '54172969', '359', '3');
INSERT INTO `npcsspawns` VALUES ('4', '794', '99091983', '289', '3');
INSERT INTO `npcsspawns` VALUES ('5', '794', '99091983', '318', '3');
INSERT INTO `npcsspawns` VALUES ('6', '794', '0', '0', '0');
INSERT INTO `npcsspawns` VALUES ('7', '794', '146744', '135', '3');
INSERT INTO `npcsspawns` VALUES ('8', '586', '144935', '242', '3');
INSERT INTO `npcsspawns` VALUES ('9', '587', '142885', '287', '3');
INSERT INTO `npcsspawns` VALUES ('10', '173', '120063489', '246', '3');
INSERT INTO `npcsspawns` VALUES ('11', '794', '146744', '164', '3');
INSERT INTO `npcsspawns` VALUES ('12', '2889', '153880835', '386', '3');
INSERT INTO `npcsspawns` VALUES ('13', '588', '143906', '149', '1');
INSERT INTO `npcsspawns` VALUES ('14', '794', '146746', '203', '3');
INSERT INTO `npcsspawns` VALUES ('15', '794', '148283', '301', '3');
INSERT INTO `npcsspawns` VALUES ('17', '794', '146746', '232', '3');
INSERT INTO `npcsspawns` VALUES ('18', '794', '99091983', '303', '3');
INSERT INTO `npcsspawns` VALUES ('19', '1088', '54172969', '228', '3');
INSERT INTO `npcsspawns` VALUES ('21', '571', '146744', '150', '3');
INSERT INTO `npcsspawns` VALUES ('22', '573', '146746', '217', '3');
INSERT INTO `npcsspawns` VALUES ('23', '718', '147767', '283', '2');
INSERT INTO `npcsspawns` VALUES ('24', '714', '149816', '239', '2');
INSERT INTO `npcsspawns` VALUES ('25', '572', '148283', '287', '3');
INSERT INTO `npcsspawns` VALUES ('26', '567', '145719', '344', '2');
INSERT INTO `npcsspawns` VALUES ('27', '568', '144695', '311', '3');
INSERT INTO `npcsspawns` VALUES ('28', '569', '146235', '240', '2');
INSERT INTO `npcsspawns` VALUES ('29', '578', '146748', '341', '3');
INSERT INTO `npcsspawns` VALUES ('30', '579', '149813', '372', '3');
INSERT INTO `npcsspawns` VALUES ('31', '574', '148278', '317', '2');
INSERT INTO `npcsspawns` VALUES ('32', '576', '145716', '269', '3');
INSERT INTO `npcsspawns` VALUES ('33', '720', '144700', '497', '3');
INSERT INTO `npcsspawns` VALUES ('34', '581', '141859', '465', '3');
INSERT INTO `npcsspawns` VALUES ('35', '1285', '54176049', '437', '3');
INSERT INTO `npcsspawns` VALUES ('36', '1300', '54165815', '271', '2');
INSERT INTO `npcsspawns` VALUES ('37', '1358', '54169427', '213', '2');
INSERT INTO `npcsspawns` VALUES ('38', '1385', '54174027', '341', '2');
INSERT INTO `npcsspawns` VALUES ('39', '1391', '60036612', '285', '2');
INSERT INTO `npcsspawns` VALUES ('40', '1236', '148752', '301', '3');
INSERT INTO `npcsspawns` VALUES ('41', '1363', '54159661', '203', '2');
INSERT INTO `npcsspawns` VALUES ('42', '1359', '68422148', '260', '3');
INSERT INTO `npcsspawns` VALUES ('43', '1034', '18874368', '165', '3');
INSERT INTO `npcsspawns` VALUES ('44', '779', '143361', '178', '1');
INSERT INTO `npcsspawns` VALUES ('45', '1036', '17828355', '431', '3');
INSERT INTO `npcsspawns` VALUES ('46', '780', '84676355', '303', '3');
INSERT INTO `npcsspawns` VALUES ('47', '798', '84679430', '218', '3');
INSERT INTO `npcsspawns` VALUES ('48', '770', '149769', '244', '2');
INSERT INTO `npcsspawns` VALUES ('49', '925', '95420420', '231', '2');
INSERT INTO `npcsspawns` VALUES ('50', '1452', '68420098', '288', '2');
INSERT INTO `npcsspawns` VALUES ('51', '2271', '88083210', '398', '3');
INSERT INTO `npcsspawns` VALUES ('52', '111', '88083210', '427', '3');
INSERT INTO `npcsspawns` VALUES ('53', '100', '54534165', '315', '3');
INSERT INTO `npcsspawns` VALUES ('54', '794', '148283', '272', '3');
INSERT INTO `npcsspawns` VALUES ('55', '1714', '54176040', '272', '2');
INSERT INTO `npcsspawns` VALUES ('56', '1713', '140641537', '473', '2');
INSERT INTO `npcsspawns` VALUES ('57', '63', '152043521', '417', '3');
INSERT INTO `npcsspawns` VALUES ('58', '756', '153881600', '344', '3');
INSERT INTO `npcsspawns` VALUES ('59', '794', '99091983', '300', '1');
INSERT INTO `npcsspawns` VALUES ('60', '1223', '83887104', '300', '3');
INSERT INTO `npcsspawns` VALUES ('539', '100', '83887104', '273', '3');
INSERT INTO `npcsspawns` VALUES ('540', '799', '25035524', '164', '3');
INSERT INTO `npcsspawns` VALUES ('541', '1901', '103548416', '345', '3');
INSERT INTO `npcsspawns` VALUES ('542', '818', '101714451', '257', '1');
INSERT INTO `npcsspawns` VALUES ('543', '800', '28443909', '150', '3');
INSERT INTO `npcsspawns` VALUES ('544', '797', '149422080', '204', '3');
INSERT INTO `npcsspawns` VALUES ('545', '712', '141836', '207', '3');
INSERT INTO `npcsspawns` VALUES ('546', '945', '22807552', '366', '1');
INSERT INTO `npcsspawns` VALUES ('547', '789', '86248450', '289', '3');
INSERT INTO `npcsspawns` VALUES ('548', '1034', '18876418', '185', '1');
INSERT INTO `npcsspawns` VALUES ('549', '784', '72221696', '213', '1');
