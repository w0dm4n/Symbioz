/*
Navicat MySQL Data Transfer

Source Server         : Local
Source Server Version : 50617
Source Host           : localhost:3306
Source Database       : symbioz

Target Server Type    : MYSQL
Target Server Version : 50617
File Encoding         : 65001

Date: 2016-04-20 12:06:24
*/

SET FOREIGN_KEY_CHECKS=0;
-- ----------------------------
-- Table structure for `dungeonsids`
-- ----------------------------
DROP TABLE IF EXISTS `dungeonsids`;
CREATE TABLE `dungeonsids` (
  `id` mediumtext,
  `name` mediumtext
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of dungeonsids
-- ----------------------------
INSERT INTO `dungeonsids` VALUES ('1', 'Donjon des Bouftous');
INSERT INTO `dungeonsids` VALUES ('3', 'Bibliothèque du Maître Corbac');
INSERT INTO `dungeonsids` VALUES ('4', 'Centre du Labyrinthe du Minotoror');
INSERT INTO `dungeonsids` VALUES ('6', 'Antre du Dragon Cochon');
INSERT INTO `dungeonsids` VALUES ('7', 'Donjon des Dragoeufs');
INSERT INTO `dungeonsids` VALUES ('8', 'Donjon des Champs');
INSERT INTO `dungeonsids` VALUES ('9', 'Domaine Ancestral');
INSERT INTO `dungeonsids` VALUES ('10', 'Clairière du Chêne Mou');
INSERT INTO `dungeonsids` VALUES ('11', 'Donjon des Blops');
INSERT INTO `dungeonsids` VALUES ('12', 'Caverne des Bulbes');
INSERT INTO `dungeonsids` VALUES ('13', 'Donjon des Bworks');
INSERT INTO `dungeonsids` VALUES ('14', 'Donjon du Bworker');
INSERT INTO `dungeonsids` VALUES ('15', 'Donjon des Canidés');
INSERT INTO `dungeonsids` VALUES ('17', 'Terrier du Wa Wabbit');
INSERT INTO `dungeonsids` VALUES ('18', 'Donjon des Craqueleurs');
INSERT INTO `dungeonsids` VALUES ('19', 'Donjon Ensablé');
INSERT INTO `dungeonsids` VALUES ('20', 'Donjon des Firefoux');
INSERT INTO `dungeonsids` VALUES ('21', 'Donjon des Forgerons');
INSERT INTO `dungeonsids` VALUES ('22', 'Donjon Fungus');
INSERT INTO `dungeonsids` VALUES ('24', 'Gelaxième Dimension');
INSERT INTO `dungeonsids` VALUES ('25', 'Grotte Hesque');
INSERT INTO `dungeonsids` VALUES ('26', 'Donjon d Incarnam');
INSERT INTO `dungeonsids` VALUES ('27', 'Donjon Kanniboul');
INSERT INTO `dungeonsids` VALUES ('28', 'Canopée du Kimbo');
INSERT INTO `dungeonsids` VALUES ('29', 'Donjon des Kitsounes');
INSERT INTO `dungeonsids` VALUES ('30', 'Caverne du Koulosse');
INSERT INTO `dungeonsids` VALUES ('31', 'Antre du Kralamoure Géant');
INSERT INTO `dungeonsids` VALUES ('32', 'Nid du Kwakwa');
INSERT INTO `dungeonsids` VALUES ('33', 'Donjon des Larves');
INSERT INTO `dungeonsids` VALUES ('34', 'Maison Fantôme');
INSERT INTO `dungeonsids` VALUES ('35', 'Laboratoire de Brumen Tinctorias');
INSERT INTO `dungeonsids` VALUES ('36', 'Donjon de Nowel');
INSERT INTO `dungeonsids` VALUES ('37', 'Caverne de Nowel');
INSERT INTO `dungeonsids` VALUES ('38', 'Maison du Papa Nowel');
INSERT INTO `dungeonsids` VALUES ('39', 'Cale de l Arche d Otomaï');
INSERT INTO `dungeonsids` VALUES ('40', 'Repaire des Pandikazes');
INSERT INTO `dungeonsids` VALUES ('41', 'Goulet du Rasboul');
INSERT INTO `dungeonsids` VALUES ('42', 'Donjon des Rats de Bonta');
INSERT INTO `dungeonsids` VALUES ('43', 'Donjon des Rats de Brâkmar');
INSERT INTO `dungeonsids` VALUES ('44', 'Donjon des Rats du Château d Amakna');
INSERT INTO `dungeonsids` VALUES ('45', 'Donjon des Scarafeuilles');
INSERT INTO `dungeonsids` VALUES ('46', 'Repaire de Skeunk');
INSERT INTO `dungeonsids` VALUES ('47', 'Donjon des Squelettes');
INSERT INTO `dungeonsids` VALUES ('48', 'Donjon des Tofus');
INSERT INTO `dungeonsids` VALUES ('49', 'Antre du Blop Multicolore Royal');
INSERT INTO `dungeonsids` VALUES ('50', 'Tofulailler Royal');
INSERT INTO `dungeonsids` VALUES ('51', 'Laboratoire du Tynril');
INSERT INTO `dungeonsids` VALUES ('52', 'Château du Wa Wabbit');
INSERT INTO `dungeonsids` VALUES ('53', 'Salle du Minotot');
INSERT INTO `dungeonsids` VALUES ('54', 'Serre du Royalmouth');
INSERT INTO `dungeonsids` VALUES ('55', 'Excavation du Mansot Royal');
INSERT INTO `dungeonsids` VALUES ('56', 'Epave du Grolandais violent');
INSERT INTO `dungeonsids` VALUES ('57', 'Hypogée de l Obsidiantre');
INSERT INTO `dungeonsids` VALUES ('58', 'Tanière Givrefoux');
INSERT INTO `dungeonsids` VALUES ('59', 'Antre du Korriandre');
INSERT INTO `dungeonsids` VALUES ('60', 'Cavernes du Kolosso');
INSERT INTO `dungeonsids` VALUES ('61', 'Antichambre des Gloursons');
INSERT INTO `dungeonsids` VALUES ('62', 'Donjon de la mine de Sakaï');
INSERT INTO `dungeonsids` VALUES ('63', 'Donjon des Tofus et du Tofu Royal');
INSERT INTO `dungeonsids` VALUES ('64', 'Repaire de Daïgoro');
INSERT INTO `dungeonsids` VALUES ('65', 'Sanctuaire des Familiers');
INSERT INTO `dungeonsids` VALUES ('66', 'Potager d Halouine');
INSERT INTO `dungeonsids` VALUES ('67', 'Transporteur de Sylargh');
INSERT INTO `dungeonsids` VALUES ('68', 'Salons privés de Klime');
INSERT INTO `dungeonsids` VALUES ('69', 'Forgefroide de Missiz Frizz');
INSERT INTO `dungeonsids` VALUES ('70', 'Laboratoire de Nileza');
INSERT INTO `dungeonsids` VALUES ('71', 'Donjon du Comte Harebourg');
INSERT INTO `dungeonsids` VALUES ('72', 'Théâtre de Dramak');
INSERT INTO `dungeonsids` VALUES ('73', 'Aquadôme de Merkator');
INSERT INTO `dungeonsids` VALUES ('74', 'Pyramide d Ombre');
INSERT INTO `dungeonsids` VALUES ('75', 'Grotte de Kanigroula');
INSERT INTO `dungeonsids` VALUES ('76', 'Palais du roi Nidas');
INSERT INTO `dungeonsids` VALUES ('77', 'Fabrique de Malléfisk');
INSERT INTO `dungeonsids` VALUES ('79', 'Galerie du Phossile');
INSERT INTO `dungeonsids` VALUES ('80', 'Volière de la Haute Truche');
INSERT INTO `dungeonsids` VALUES ('81', 'Ring du Capitaine Ekarlatte');
INSERT INTO `dungeonsids` VALUES ('82', 'Cave du Toxoliath');
INSERT INTO `dungeonsids` VALUES ('83', 'Trône de la Cour Sombre');
INSERT INTO `dungeonsids` VALUES ('84', 'Ventre de la Baleine');
INSERT INTO `dungeonsids` VALUES ('85', 'Mégalithe de Fraktale');
INSERT INTO `dungeonsids` VALUES ('86', 'Horologium de XLII');
INSERT INTO `dungeonsids` VALUES ('87', 'Œil de Vortex');
INSERT INTO `dungeonsids` VALUES ('88', 'Cache de Kankreblath');
INSERT INTO `dungeonsids` VALUES ('89', 'Antre de la Reine Nyée');
INSERT INTO `dungeonsids` VALUES ('90', 'Crypte de Kardorim');
