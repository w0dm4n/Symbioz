/*
Navicat MySQL Data Transfer

Source Server         : localhost_3306
Source Server Version : 50520
Source Host           : localhost:3306
Source Database       : symbioz

Target Server Type    : MYSQL
Target Server Version : 50520
File Encoding         : 65001

Date: 2016-06-01 23:37:51
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `mapstriggers`
-- ----------------------------
DROP TABLE IF EXISTS `mapstriggers`;
CREATE TABLE `mapstriggers` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `BaseMapId` int(11) DEFAULT NULL,
  `BaseCellId` int(11) DEFAULT NULL,
  `NextMapId` int(11) DEFAULT NULL,
  `NextCellId` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=421 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of mapstriggers
-- ----------------------------
INSERT INTO `mapstriggers` VALUES ('1', '81527297', '427', '80218116', '332');
INSERT INTO `mapstriggers` VALUES ('2', '81527297', '427', '80218116', '332');
INSERT INTO `mapstriggers` VALUES ('3', '81529347', '341', '80216578', '299');
INSERT INTO `mapstriggers` VALUES ('4', '81528325', '316', '80216066', '315');
INSERT INTO `mapstriggers` VALUES ('5', '81527301', '316', '80216066', '355');
INSERT INTO `mapstriggers` VALUES ('6', '81529349', '344', '80216064', '284');
INSERT INTO `mapstriggers` VALUES ('7', '81527299', '424', '80218626', '328');
INSERT INTO `mapstriggers` VALUES ('8', '81527299', '438', '80218626', '328');
INSERT INTO `mapstriggers` VALUES ('9', '81528323', '311', '81527299', '340');
INSERT INTO `mapstriggers` VALUES ('10', '83628034', '395', '84672519', '385');
INSERT INTO `mapstriggers` VALUES ('11', '83890182', '401', '84673543', '211');
INSERT INTO `mapstriggers` VALUES ('12', '83364864', '352', '84674055', '397');
INSERT INTO `mapstriggers` VALUES ('13', '103547392', '361', '84675590', '242');
INSERT INTO `mapstriggers` VALUES ('14', '83625986', '397', '84675078', '384');
INSERT INTO `mapstriggers` VALUES ('15', '83625986', '382', '84675078', '384');
INSERT INTO `mapstriggers` VALUES ('16', '83887104', '396', '84674566', '317');
INSERT INTO `mapstriggers` VALUES ('17', '83629056', '410', '84673542', '269');
INSERT INTO `mapstriggers` VALUES ('18', '83891204', '402', '84673030', '412');
INSERT INTO `mapstriggers` VALUES ('19', '83629058', '410', '84673541', '383');
INSERT INTO `mapstriggers` VALUES ('20', '83892232', '429', '84674053', '282');
INSERT INTO `mapstriggers` VALUES ('21', '83890176', '366', '84675077', '368');
INSERT INTO `mapstriggers` VALUES ('22', '83890176', '443', '84675077', '346');
INSERT INTO `mapstriggers` VALUES ('23', '83362816', '352', '84675589', '397');
INSERT INTO `mapstriggers` VALUES ('24', '83363842', '352', '84674564', '410');
INSERT INTO `mapstriggers` VALUES ('25', '83362818', '352', '84675076', '354');
INSERT INTO `mapstriggers` VALUES ('26', '83365888', '352', '84675588', '369');
INSERT INTO `mapstriggers` VALUES ('27', '101716483', '522', '84675587', '304');
INSERT INTO `mapstriggers` VALUES ('28', '83888130', '401', '84674563', '268');
INSERT INTO `mapstriggers` VALUES ('29', '83887110', '437', '84673539', '370');
INSERT INTO `mapstriggers` VALUES ('30', '83363840', '352', '84672514', '382');
INSERT INTO `mapstriggers` VALUES ('31', '83889152', '367', '84673026', '357');
INSERT INTO `mapstriggers` VALUES ('32', '83365890', '352', '84674050', '399');
INSERT INTO `mapstriggers` VALUES ('33', '83628032', '396', '84675074', '425');
INSERT INTO `mapstriggers` VALUES ('34', '83624960', '369', '84676098', '244');
INSERT INTO `mapstriggers` VALUES ('35', '83624962', '355', '84675585', '299');
INSERT INTO `mapstriggers` VALUES ('36', '83624962', '370', '84675585', '299');
INSERT INTO `mapstriggers` VALUES ('37', '83623936', '326', '84673537', '382');
INSERT INTO `mapstriggers` VALUES ('38', '83627008', '325', '84672513', '328');
INSERT INTO `mapstriggers` VALUES ('39', '83887106', '471', '84804097', '299');
INSERT INTO `mapstriggers` VALUES ('40', '83887106', '458', '84804097', '299');
INSERT INTO `mapstriggers` VALUES ('41', '83361792', '352', '84672512', '343');
INSERT INTO `mapstriggers` VALUES ('42', '83625984', '424', '84673024', '454');
INSERT INTO `mapstriggers` VALUES ('43', '67371008', '465', '84673536', '413');
INSERT INTO `mapstriggers` VALUES ('44', '67371008', '451', '84673536', '413');
INSERT INTO `mapstriggers` VALUES ('45', '83623938', '424', '84676355', '289');
INSERT INTO `mapstriggers` VALUES ('46', '17042432', '350', '88083734', '381');
INSERT INTO `mapstriggers` VALUES ('47', '102236681', '536', '84676358', '219');
INSERT INTO `mapstriggers` VALUES ('48', '101716483', '171', '101715459', '487');
INSERT INTO `mapstriggers` VALUES ('49', '101715459', '501', '101716483', '185');
INSERT INTO `mapstriggers` VALUES ('50', '101715461', '462', '101715463', '134');
INSERT INTO `mapstriggers` VALUES ('51', '101715461', '477', '101715463', '134');
INSERT INTO `mapstriggers` VALUES ('52', '101715463', '121', '101715461', '449');
INSERT INTO `mapstriggers` VALUES ('53', '101715463', '135', '101715461', '449');
INSERT INTO `mapstriggers` VALUES ('54', '7077888', '458', '264', '228');
INSERT INTO `mapstriggers` VALUES ('55', '7077888', '269', '7078912', '414');
INSERT INTO `mapstriggers` VALUES ('56', '7078912', '429', '7077888', '284');
INSERT INTO `mapstriggers` VALUES ('57', '4456450', '417', '132359', '208');
INSERT INTO `mapstriggers` VALUES ('58', '4456450', '394', '132359', '314');
INSERT INTO `mapstriggers` VALUES ('59', '69208064', '511', '69209346', '301');
INSERT INTO `mapstriggers` VALUES ('60', '17039360', '382', '88080668', '342');
INSERT INTO `mapstriggers` VALUES ('61', '17039360', '397', '88080668', '342');
INSERT INTO `mapstriggers` VALUES ('62', '4461568', '335', '262', '315');
INSERT INTO `mapstriggers` VALUES ('63', '4461568', '349', '262', '315');
INSERT INTO `mapstriggers` VALUES ('64', '4461568', '363', '262', '315');
INSERT INTO `mapstriggers` VALUES ('65', '17048576', '456', '88212763', '443');
INSERT INTO `mapstriggers` VALUES ('66', '17048576', '469', '88212763', '443');
INSERT INTO `mapstriggers` VALUES ('67', '17048580', '423', '17048578', '275');
INSERT INTO `mapstriggers` VALUES ('68', '17048580', '437', '17048578', '275');
INSERT INTO `mapstriggers` VALUES ('69', '17048578', '469', '17048582', '331');
INSERT INTO `mapstriggers` VALUES ('70', '17048578', '456', '17048582', '331');
INSERT INTO `mapstriggers` VALUES ('71', '17048584', '385', '17048582', '297');
INSERT INTO `mapstriggers` VALUES ('72', '99353612', '369', '88213274', '227');
INSERT INTO `mapstriggers` VALUES ('73', '97255955', '478', '88212250', '262');
INSERT INTO `mapstriggers` VALUES ('74', '97255955', '492', '88212250', '262');
INSERT INTO `mapstriggers` VALUES ('75', '97255955', '512', '97256979', '312');
INSERT INTO `mapstriggers` VALUES ('76', '97256979', '297', '97255955', '497');
INSERT INTO `mapstriggers` VALUES ('77', '99352590', '443', '88212250', '412');
INSERT INTO `mapstriggers` VALUES ('78', '78905344', '386', '78905350', '146');
INSERT INTO `mapstriggers` VALUES ('79', '78905348', '395', '78905344', '289');
INSERT INTO `mapstriggers` VALUES ('80', '78905346', '333', '78905344', '284');
INSERT INTO `mapstriggers` VALUES ('81', '99092997', '384', '88080665', '414');
INSERT INTO `mapstriggers` VALUES ('82', '99094021', '314', '99092997', '317');
INSERT INTO `mapstriggers` VALUES ('83', '99094021', '328', '99092997', '317');
INSERT INTO `mapstriggers` VALUES ('84', '99095051', '410', '88081177', '230');
INSERT INTO `mapstriggers` VALUES ('85', '99090953', '344', '99094025', '254');
INSERT INTO `mapstriggers` VALUES ('86', '99090953', '355', '88081176', '287');
INSERT INTO `mapstriggers` VALUES ('87', '99091977', '429', '99090953', '299');
INSERT INTO `mapstriggers` VALUES ('88', '99090957', '471', '88212247', '343');
INSERT INTO `mapstriggers` VALUES ('89', '99090957', '458', '88212247', '343');
INSERT INTO `mapstriggers` VALUES ('90', '99096069', '355', '88213272', '315');
INSERT INTO `mapstriggers` VALUES ('91', '17046528', '442', '88214295', '399');
INSERT INTO `mapstriggers` VALUES ('92', '17046528', '388', '88214295', '331');
INSERT INTO `mapstriggers` VALUES ('93', '17046534', '473', '17046532', '298');
INSERT INTO `mapstriggers` VALUES ('94', '99090955', '355', '88081174', '414');
INSERT INTO `mapstriggers` VALUES ('95', '99090955', '370', '88081174', '414');
INSERT INTO `mapstriggers` VALUES ('96', '17045504', '397', '88081686', '387');
INSERT INTO `mapstriggers` VALUES ('97', '17045504', '411', '88081686', '387');
INSERT INTO `mapstriggers` VALUES ('98', '17045506', '410', '17045504', '359');
INSERT INTO `mapstriggers` VALUES ('99', '17045508', '465', '17045506', '289');
INSERT INTO `mapstriggers` VALUES ('100', '17045510', '395', '17045508', '275');
INSERT INTO `mapstriggers` VALUES ('101', '17045510', '409', '17045508', '275');
INSERT INTO `mapstriggers` VALUES ('102', '99353600', '424', '88081687', '357');
INSERT INTO `mapstriggers` VALUES ('103', '99091969', '351', '88082198', '357');
INSERT INTO `mapstriggers` VALUES ('104', '99352586', '370', '88083223', '300');
INSERT INTO `mapstriggers` VALUES ('105', '99352586', '355', '88083223', '300');
INSERT INTO `mapstriggers` VALUES ('106', '99356682', '312', '88082710', '328');
INSERT INTO `mapstriggers` VALUES ('107', '99357708', '342', '99356682', '345');
INSERT INTO `mapstriggers` VALUES ('108', '17042432', '548', '88083734', '440');
INSERT INTO `mapstriggers` VALUES ('109', '17042432', '535', '88083734', '440');
INSERT INTO `mapstriggers` VALUES ('110', '17042432', '549', '88083734', '440');
INSERT INTO `mapstriggers` VALUES ('111', '17042432', '536', '88083734', '440');
INSERT INTO `mapstriggers` VALUES ('112', '17042432', '550', '88083734', '440');
INSERT INTO `mapstriggers` VALUES ('113', '17042432', '280', '88083734', '381');
INSERT INTO `mapstriggers` VALUES ('114', '17042432', '294', '88083734', '381');
INSERT INTO `mapstriggers` VALUES ('115', '17042432', '308', '88083734', '381');
INSERT INTO `mapstriggers` VALUES ('116', '17042432', '322', '88083734', '381');
INSERT INTO `mapstriggers` VALUES ('117', '17042432', '336', '88083734', '381');
INSERT INTO `mapstriggers` VALUES ('118', '81529345', '358', '80218116', '345');
INSERT INTO `mapstriggers` VALUES ('119', '17042432', '364', '88083734', '381');
INSERT INTO `mapstriggers` VALUES ('120', '17042432', '378', '88083734', '381');
INSERT INTO `mapstriggers` VALUES ('121', '17042432', '392', '88083734', '381');
INSERT INTO `mapstriggers` VALUES ('122', '17042440', '401', '17042434', '352');
INSERT INTO `mapstriggers` VALUES ('123', '17041411', '424', '88084245', '274');
INSERT INTO `mapstriggers` VALUES ('124', '17041409', '458', '17041411', '257');
INSERT INTO `mapstriggers` VALUES ('125', '17041413', '424', '17041409', '290');
INSERT INTO `mapstriggers` VALUES ('126', '17041415', '415', '17041409', '229');
INSERT INTO `mapstriggers` VALUES ('127', '96470786', '452', '95423492', '344');
INSERT INTO `mapstriggers` VALUES ('128', '96470786', '466', '95423492', '344');
INSERT INTO `mapstriggers` VALUES ('129', '96470528', '494', '96470786', '426');
INSERT INTO `mapstriggers` VALUES ('130', '96470528', '499', '96471552', '325');
INSERT INTO `mapstriggers` VALUES ('131', '96470530', '473', '96471554', '354');
INSERT INTO `mapstriggers` VALUES ('132', '96470530', '487', '96471554', '354');
INSERT INTO `mapstriggers` VALUES ('133', '96469506', '499', '96470530', '212');
INSERT INTO `mapstriggers` VALUES ('134', '96469504', '499', '96470528', '354');
INSERT INTO `mapstriggers` VALUES ('135', '87821827', '372', '68420615', '314');
INSERT INTO `mapstriggers` VALUES ('136', '78909440', '386', '78909446', '146');
INSERT INTO `mapstriggers` VALUES ('137', '78909444', '409', '78909440', '289');
INSERT INTO `mapstriggers` VALUES ('138', '78909442', '333', '78909440', '284');
INSERT INTO `mapstriggers` VALUES ('139', '87820801', '414', '68420099', '373');
INSERT INTO `mapstriggers` VALUES ('140', '95685125', '436', '68551681', '370');
INSERT INTO `mapstriggers` VALUES ('141', '95685125', '451', '68551681', '370');
INSERT INTO `mapstriggers` VALUES ('142', '95685125', '465', '68551681', '370');
INSERT INTO `mapstriggers` VALUES ('143', '95685125', '443', '95686149', '241');
INSERT INTO `mapstriggers` VALUES ('144', '95683073', '487', '68552193', '386');
INSERT INTO `mapstriggers` VALUES ('145', '95683073', '473', '68552193', '386');
INSERT INTO `mapstriggers` VALUES ('146', '95684097', '458', '95683073', '240');
INSERT INTO `mapstriggers` VALUES ('147', '95684097', '444', '95683073', '240');
INSERT INTO `mapstriggers` VALUES ('148', '95686145', '516', '95684097', '256');
INSERT INTO `mapstriggers` VALUES ('149', '95686145', '502', '95684097', '256');
INSERT INTO `mapstriggers` VALUES ('150', '95683075', '528', '95686145', '227');
INSERT INTO `mapstriggers` VALUES ('151', '95683075', '515', '95686145', '227');
INSERT INTO `mapstriggers` VALUES ('152', '87819777', '410', '68420611', '275');
INSERT INTO `mapstriggers` VALUES ('153', '99094023', '465', '88083220', '426');
INSERT INTO `mapstriggers` VALUES ('154', '101974016', '429', '88082708', '384');
INSERT INTO `mapstriggers` VALUES ('155', '101974016', '415', '88082708', '384');
INSERT INTO `mapstriggers` VALUES ('156', '8912911', '485', '8913935', '296');
INSERT INTO `mapstriggers` VALUES ('157', '78905344', '386', '78905350', '146');
INSERT INTO `mapstriggers` VALUES ('158', '78905348', '395', '78905344', '289');
INSERT INTO `mapstriggers` VALUES ('159', '78905346', '333', '78905344', '284');
INSERT INTO `mapstriggers` VALUES ('160', '101450251', '451', '84805890', '329');
INSERT INTO `mapstriggers` VALUES ('161', '101450251', '465', '84805890', '329');
INSERT INTO `mapstriggers` VALUES ('162', '101453321', '466', '101450251', '303');
INSERT INTO `mapstriggers` VALUES ('163', '101451267', '437', '101453321', '416');
INSERT INTO `mapstriggers` VALUES ('164', '101451267', '452', '101453321', '416');
INSERT INTO `mapstriggers` VALUES ('165', '101450247', '437', '101453321', '373');
INSERT INTO `mapstriggers` VALUES ('166', '101450247', '452', '101453321', '373');
INSERT INTO `mapstriggers` VALUES ('167', '101453319', '437', '101453321', '315');
INSERT INTO `mapstriggers` VALUES ('168', '101453319', '452', '101453321', '315');
INSERT INTO `mapstriggers` VALUES ('169', '101452295', '437', '101453321', '286');
INSERT INTO `mapstriggers` VALUES ('170', '101452295', '452', '101453321', '286');
INSERT INTO `mapstriggers` VALUES ('171', '81788928', '478', '138013', '345');
INSERT INTO `mapstriggers` VALUES ('172', '81788928', '492', '138013', '345');
INSERT INTO `mapstriggers` VALUES ('173', '81788930', '443', '81788928', '215');
INSERT INTO `mapstriggers` VALUES ('174', '81788930', '430', '81788928', '215');
INSERT INTO `mapstriggers` VALUES ('175', '81789952', '436', '153878788', '328');
INSERT INTO `mapstriggers` VALUES ('176', '81789952', '465', '153878788', '328');
INSERT INTO `mapstriggers` VALUES ('177', '50331648', '531', '135976', '323');
INSERT INTO `mapstriggers` VALUES ('178', '50331648', '517', '135976', '323');
INSERT INTO `mapstriggers` VALUES ('179', '50331648', '503', '135976', '323');
INSERT INTO `mapstriggers` VALUES ('180', '50331648', '489', '135976', '323');
INSERT INTO `mapstriggers` VALUES ('181', '50331648', '475', '135976', '323');
INSERT INTO `mapstriggers` VALUES ('182', '41420800', '399', '134440', '236');
INSERT INTO `mapstriggers` VALUES ('183', '99096073', '329', '99095049', '230');
INSERT INTO `mapstriggers` VALUES ('184', '8129542', '424', '12580', '284');
INSERT INTO `mapstriggers` VALUES ('185', '8129542', '409', '12580', '284');
INSERT INTO `mapstriggers` VALUES ('186', '91753985', '480', '90703872', '316');
INSERT INTO `mapstriggers` VALUES ('187', '91753985', '494', '90703872', '316');
INSERT INTO `mapstriggers` VALUES ('188', '39845888', '451', '144681', '428');
INSERT INTO `mapstriggers` VALUES ('189', '39845888', '465', '144681', '428');
INSERT INTO `mapstriggers` VALUES ('190', '99095049', '376', '88081176', '403');
INSERT INTO `mapstriggers` VALUES ('191', '99095049', '389', '88081176', '403');
INSERT INTO `mapstriggers` VALUES ('192', '41418752', '409', '143160', '327');
INSERT INTO `mapstriggers` VALUES ('193', '50331650', '367', '135974', '382');
INSERT INTO `mapstriggers` VALUES ('194', '83627010', '430', '84804355', '326');
INSERT INTO `mapstriggers` VALUES ('195', '41157632', '397', '133901', '314');
INSERT INTO `mapstriggers` VALUES ('196', '4459522', '451', '132870', '330');
INSERT INTO `mapstriggers` VALUES ('197', '132360', '257', '17051648', '386');
INSERT INTO `mapstriggers` VALUES ('198', '17051648', '401', '132360', '271');
INSERT INTO `mapstriggers` VALUES ('199', '17051648', '414', '132360', '271');
INSERT INTO `mapstriggers` VALUES ('200', '17050626', '446', '17051648', '270');
INSERT INTO `mapstriggers` VALUES ('201', '17050626', '460', '17051648', '270');
INSERT INTO `mapstriggers` VALUES ('202', '99092993', '212', '99091969', '282');
INSERT INTO `mapstriggers` VALUES ('203', '99092993', '199', '99091969', '268');
INSERT INTO `mapstriggers` VALUES ('204', '99090945', '428', '88080664', '325');
INSERT INTO `mapstriggers` VALUES ('205', '99090945', '414', '88080664', '325');
INSERT INTO `mapstriggers` VALUES ('206', '99353610', '370', '88212757', '268');
INSERT INTO `mapstriggers` VALUES ('207', '99353610', '355', '88212757', '268');
INSERT INTO `mapstriggers` VALUES ('208', '99354634', '397', '99353610', '331');
INSERT INTO `mapstriggers` VALUES ('209', '99354634', '372', '88212757', '218');
INSERT INTO `mapstriggers` VALUES ('210', '99354634', '358', '88212757', '218');
INSERT INTO `mapstriggers` VALUES ('211', '99356672', '382', '88212245', '301');
INSERT INTO `mapstriggers` VALUES ('212', '99356672', '397', '88212245', '301');
INSERT INTO `mapstriggers` VALUES ('213', '99356680', '440', '88081173', '401');
INSERT INTO `mapstriggers` VALUES ('214', '99096067', '399', '88082197', '289');
INSERT INTO `mapstriggers` VALUES ('215', '99096067', '384', '88082197', '289');
INSERT INTO `mapstriggers` VALUES ('216', '99357700', '415', '88084755', '327');
INSERT INTO `mapstriggers` VALUES ('217', '99354636', '369', '88212754', '274');
INSERT INTO `mapstriggers` VALUES ('218', '99352582', '430', '88082191', '326');
INSERT INTO `mapstriggers` VALUES ('219', '99095047', '414', '88082187', '357');
INSERT INTO `mapstriggers` VALUES ('220', '99095047', '428', '88082187', '357');
INSERT INTO `mapstriggers` VALUES ('221', '99352576', '424', '88082186', '358');
INSERT INTO `mapstriggers` VALUES ('222', '99357702', '326', '88081675', '271');
INSERT INTO `mapstriggers` VALUES ('223', '99355650', '326', '88080650', '244');
INSERT INTO `mapstriggers` VALUES ('224', '99352584', '414', '88080647', '369');
INSERT INTO `mapstriggers` VALUES ('225', '99355654', '430', '88083207', '343');
INSERT INTO `mapstriggers` VALUES ('226', '99096071', '384', '88084738', '302');
INSERT INTO `mapstriggers` VALUES ('227', '99096071', '399', '88084738', '302');
INSERT INTO `mapstriggers` VALUES ('228', '99354626', '326', '88087299', '259');
INSERT INTO `mapstriggers` VALUES ('229', '99354638', '424', '88087298', '359');
INSERT INTO `mapstriggers` VALUES ('230', '76809732', '397', '76288257', '214');
INSERT INTO `mapstriggers` VALUES ('231', '8913935', '282', '8912911', '470');
INSERT INTO `mapstriggers` VALUES ('232', '8912911', '424', '144931', '175');
INSERT INTO `mapstriggers` VALUES ('233', '8912911', '409', '144931', '175');
INSERT INTO `mapstriggers` VALUES ('234', '8913935', '409', '144931', '218');
INSERT INTO `mapstriggers` VALUES ('235', '8913935', '424', '144931', '218');
INSERT INTO `mapstriggers` VALUES ('236', '8914959', '409', '144931', '262');
INSERT INTO `mapstriggers` VALUES ('237', '8914959', '424', '144931', '262');
INSERT INTO `mapstriggers` VALUES ('238', '8913935', '485', '8914959', '296');
INSERT INTO `mapstriggers` VALUES ('239', '8914959', '282', '8913935', '470');
INSERT INTO `mapstriggers` VALUES ('240', '24380418', '436', '24379394', '247');
INSERT INTO `mapstriggers` VALUES ('241', '24379394', '409', '155157', '355');
INSERT INTO `mapstriggers` VALUES ('242', '54534165', '424', '54172457', '372');
INSERT INTO `mapstriggers` VALUES ('243', '54534165', '409', '54172457', '372');
INSERT INTO `mapstriggers` VALUES ('244', '2883593', '485', '2884617', '296');
INSERT INTO `mapstriggers` VALUES ('245', '2883593', '424', '147254', '296');
INSERT INTO `mapstriggers` VALUES ('246', '2883593', '409', '147254', '296');
INSERT INTO `mapstriggers` VALUES ('247', '2884617', '282', '2883593', '470');
INSERT INTO `mapstriggers` VALUES ('248', '2884617', '409', '147254', '340');
INSERT INTO `mapstriggers` VALUES ('249', '2884617', '424', '147254', '340');
INSERT INTO `mapstriggers` VALUES ('250', '2884617', '485', '2885641', '296');
INSERT INTO `mapstriggers` VALUES ('251', '2885641', '282', '2884617', '470');
INSERT INTO `mapstriggers` VALUES ('252', '2885641', '424', '147254', '383');
INSERT INTO `mapstriggers` VALUES ('253', '2885641', '409', '147254', '383');
INSERT INTO `mapstriggers` VALUES ('254', '6291461', '410', '146226', '214');
INSERT INTO `mapstriggers` VALUES ('255', '91750917', '442', '95424000', '244');
INSERT INTO `mapstriggers` VALUES ('256', '86248450', '415', '84411392', '215');
INSERT INTO `mapstriggers` VALUES ('257', '17826562', '157', '140316', '356');
INSERT INTO `mapstriggers` VALUES ('258', '3145738', '415', '149309', '200');
INSERT INTO `mapstriggers` VALUES ('259', '12845062', '410', '146467', '271');
INSERT INTO `mapstriggers` VALUES ('260', '84935175', '410', '73400323', '330');
INSERT INTO `mapstriggers` VALUES ('261', '84935175', '425', '73400323', '330');
INSERT INTO `mapstriggers` VALUES ('262', '3145736', '424', '148797', '188');
INSERT INTO `mapstriggers` VALUES ('263', '3145733', '396', '148797', '523');
INSERT INTO `mapstriggers` VALUES ('264', '3145732', '415', '148796', '285');
INSERT INTO `mapstriggers` VALUES ('265', '3145732', '492', '148796', '294');
INSERT INTO `mapstriggers` VALUES ('266', '4718600', '451', '148787', '304');
INSERT INTO `mapstriggers` VALUES ('267', '2884110', '506', '147769', '372');
INSERT INTO `mapstriggers` VALUES ('268', '2884110', '491', '147769', '372');
INSERT INTO `mapstriggers` VALUES ('269', '74186754', '430', '2884110', '241');
INSERT INTO `mapstriggers` VALUES ('270', '4981250', '539', '2884110', '275');
INSERT INTO `mapstriggers` VALUES ('271', '5505024', '436', '5508102', '166');
INSERT INTO `mapstriggers` VALUES ('272', '5505024', '431', '5507072', '158');
INSERT INTO `mapstriggers` VALUES ('273', '5507072', '144', '5505024', '416');
INSERT INTO `mapstriggers` VALUES ('274', '5507072', '208', '5508096', '451');
INSERT INTO `mapstriggers` VALUES ('275', '5508096', '464', '5507072', '221');
INSERT INTO `mapstriggers` VALUES ('276', '5505024', '220', '5505026', '424');
INSERT INTO `mapstriggers` VALUES ('277', '5505026', '437', '5505024', '234');
INSERT INTO `mapstriggers` VALUES ('278', '5505024', '213', '5506050', '430');
INSERT INTO `mapstriggers` VALUES ('279', '5506050', '444', '5505024', '228');
INSERT INTO `mapstriggers` VALUES ('280', '5507074', '495', '5508098', '348');
INSERT INTO `mapstriggers` VALUES ('281', '5508098', '334', '5507074', '482');
INSERT INTO `mapstriggers` VALUES ('282', '5508100', '472', '5505030', '228');
INSERT INTO `mapstriggers` VALUES ('283', '5508100', '486', '5505030', '241');
INSERT INTO `mapstriggers` VALUES ('284', '5505030', '227', '5508100', '471');
INSERT INTO `mapstriggers` VALUES ('285', '5505030', '213', '5508100', '458');
INSERT INTO `mapstriggers` VALUES ('286', '5505028', '459', '5506054', '269');
INSERT INTO `mapstriggers` VALUES ('287', '5506054', '255', '5505028', '444');
INSERT INTO `mapstriggers` VALUES ('288', '4719111', '409', '147764', '318');
INSERT INTO `mapstriggers` VALUES ('289', '6291462', '451', '145714', '258');
INSERT INTO `mapstriggers` VALUES ('290', '6291462', '458', '145714', '262');
INSERT INTO `mapstriggers` VALUES ('291', '6291463', '435', '145202', '246');
INSERT INTO `mapstriggers` VALUES ('292', '2884113', '382', '146229', '303');
INSERT INTO `mapstriggers` VALUES ('293', '2883603', '451', '147767', '301');
INSERT INTO `mapstriggers` VALUES ('294', '14155780', '479', '148793', '389');
INSERT INTO `mapstriggers` VALUES ('295', '14155780', '492', '148793', '389');
INSERT INTO `mapstriggers` VALUES ('296', '14155780', '477', '148793', '400');
INSERT INTO `mapstriggers` VALUES ('297', '14155780', '491', '148793', '400');
INSERT INTO `mapstriggers` VALUES ('298', '14155780', '459', '14156804', '228');
INSERT INTO `mapstriggers` VALUES ('299', '14155780', '445', '14156804', '228');
INSERT INTO `mapstriggers` VALUES ('300', '14158852', '424', '14156804', '318');
INSERT INTO `mapstriggers` VALUES ('301', '14158852', '438', '14156804', '318');
INSERT INTO `mapstriggers` VALUES ('302', '14156802', '424', '14157824', '304');
INSERT INTO `mapstriggers` VALUES ('303', '14156802', '438', '14157824', '304');
INSERT INTO `mapstriggers` VALUES ('304', '14158848', '460', '14157824', '241');
INSERT INTO `mapstriggers` VALUES ('305', '14158848', '473', '14157824', '241');
INSERT INTO `mapstriggers` VALUES ('306', '14155778', '445', '14158848', '213');
INSERT INTO `mapstriggers` VALUES ('307', '14155778', '459', '14158848', '213');
INSERT INTO `mapstriggers` VALUES ('308', '14155776', '424', '14155778', '289');
INSERT INTO `mapstriggers` VALUES ('309', '14155776', '438', '14155778', '289');
INSERT INTO `mapstriggers` VALUES ('310', '14155776', '473', '14156800', '213');
INSERT INTO `mapstriggers` VALUES ('311', '14155776', '460', '14156800', '213');
INSERT INTO `mapstriggers` VALUES ('312', '14156800', '460', '14156802', '228');
INSERT INTO `mapstriggers` VALUES ('313', '14156800', '473', '14156802', '228');
INSERT INTO `mapstriggers` VALUES ('314', '14157826', '459', '14157828', '228');
INSERT INTO `mapstriggers` VALUES ('315', '14157826', '445', '14157828', '228');
INSERT INTO `mapstriggers` VALUES ('316', '14157826', '438', '14158850', '318');
INSERT INTO `mapstriggers` VALUES ('317', '14157826', '453', '14158850', '318');
INSERT INTO `mapstriggers` VALUES ('318', '14157828', '445', '14158852', '228');
INSERT INTO `mapstriggers` VALUES ('319', '14157828', '459', '14158852', '228');
INSERT INTO `mapstriggers` VALUES ('320', '14158850', '445', '14155780', '228');
INSERT INTO `mapstriggers` VALUES ('321', '14158850', '459', '14155780', '228');
INSERT INTO `mapstriggers` VALUES ('322', '14157830', '445', '14158854', '228');
INSERT INTO `mapstriggers` VALUES ('323', '14157830', '459', '14158854', '228');
INSERT INTO `mapstriggers` VALUES ('324', '14156808', '424', '14158854', '303');
INSERT INTO `mapstriggers` VALUES ('325', '14156808', '438', '14158854', '303');
INSERT INTO `mapstriggers` VALUES ('326', '14155784', '445', '14156808', '228');
INSERT INTO `mapstriggers` VALUES ('327', '14155784', '459', '14156808', '228');
INSERT INTO `mapstriggers` VALUES ('328', '14155782', '445', '14155784', '228');
INSERT INTO `mapstriggers` VALUES ('329', '14155782', '459', '14155784', '228');
INSERT INTO `mapstriggers` VALUES ('330', '14155782', '424', '14156806', '318');
INSERT INTO `mapstriggers` VALUES ('331', '14155782', '409', '14156806', '318');
INSERT INTO `mapstriggers` VALUES ('332', '14156806', '445', '14157830', '228');
INSERT INTO `mapstriggers` VALUES ('333', '14156806', '459', '14157830', '228');
INSERT INTO `mapstriggers` VALUES ('334', '14158856', '445', '14155786', '228');
INSERT INTO `mapstriggers` VALUES ('335', '14158856', '459', '14155786', '228');
INSERT INTO `mapstriggers` VALUES ('336', '14157832', '424', '14158856', '289');
INSERT INTO `mapstriggers` VALUES ('337', '14157832', '438', '14158856', '289');
INSERT INTO `mapstriggers` VALUES ('338', '14157832', '445', '14157834', '228');
INSERT INTO `mapstriggers` VALUES ('339', '14157832', '459', '14157834', '228');
INSERT INTO `mapstriggers` VALUES ('340', '14157834', '445', '14158858', '228');
INSERT INTO `mapstriggers` VALUES ('341', '14157834', '459', '14158858', '228');
INSERT INTO `mapstriggers` VALUES ('342', '14158858', '438', '14156810', '303');
INSERT INTO `mapstriggers` VALUES ('343', '14158858', '453', '14156810', '303');
INSERT INTO `mapstriggers` VALUES ('344', '14155786', '445', '14156810', '228');
INSERT INTO `mapstriggers` VALUES ('345', '14155786', '459', '14156810', '228');
INSERT INTO `mapstriggers` VALUES ('346', '2883602', '478', '148282', '286');
INSERT INTO `mapstriggers` VALUES ('347', '6816261', '457', '146237', '468');
INSERT INTO `mapstriggers` VALUES ('348', '6816261', '450', '146237', '478');
INSERT INTO `mapstriggers` VALUES ('349', '6815750', '437', '145725', '143');
INSERT INTO `mapstriggers` VALUES ('350', '6554630', '396', '6555142', '316');
INSERT INTO `mapstriggers` VALUES ('351', '6554630', '410', '6555142', '316');
INSERT INTO `mapstriggers` VALUES ('352', '6554118', '450', '145213', '274');
INSERT INTO `mapstriggers` VALUES ('353', '6554118', '432', '144701', '308');
INSERT INTO `mapstriggers` VALUES ('354', '6553607', '408', '144700', '395');
INSERT INTO `mapstriggers` VALUES ('355', '24908036', '110', '144700', '440');
INSERT INTO `mapstriggers` VALUES ('356', '6554631', '464', '144698', '528');
INSERT INTO `mapstriggers` VALUES ('357', '7340038', '408', '145719', '510');
INSERT INTO `mapstriggers` VALUES ('358', '7340549', '478', '145208', '394');
INSERT INTO `mapstriggers` VALUES ('359', '7340549', '415', '145208', '398');
INSERT INTO `mapstriggers` VALUES ('360', '6554631', '464', '144698', '528');
INSERT INTO `mapstriggers` VALUES ('361', '7864328', '424', '146234', '366');
INSERT INTO `mapstriggers` VALUES ('362', '7864326', '437', '146233', '368');
INSERT INTO `mapstriggers` VALUES ('363', '7864326', '430', '146233', '372');
INSERT INTO `mapstriggers` VALUES ('364', '7864327', '396', '146232', '357');
INSERT INTO `mapstriggers` VALUES ('365', '2884629', '409', '149812', '353');
INSERT INTO `mapstriggers` VALUES ('366', '17044483', '451', '88212244', '414');
INSERT INTO `mapstriggers` VALUES ('367', '17044483', '465', '88212244', '414');
INSERT INTO `mapstriggers` VALUES ('368', '17044485', '436', '17044483', '235');
INSERT INTO `mapstriggers` VALUES ('369', '17044487', '459', '17044485', '282');
INSERT INTO `mapstriggers` VALUES ('370', '17044481', '472', '17044483', '311');
INSERT INTO `mapstriggers` VALUES ('371', '17047556', '410', '88080660', '384');
INSERT INTO `mapstriggers` VALUES ('372', '17047556', '425', '88080660', '384');
INSERT INTO `mapstriggers` VALUES ('373', '17047552', '352', '17047556', '302');
INSERT INTO `mapstriggers` VALUES ('374', '17047552', '367', '17047556', '302');
INSERT INTO `mapstriggers` VALUES ('375', '17047556', '415', '17047554', '284');
INSERT INTO `mapstriggers` VALUES ('376', '17047556', '429', '17047554', '284');
INSERT INTO `mapstriggers` VALUES ('377', '67108864', '423', '82314240', '289');
INSERT INTO `mapstriggers` VALUES ('378', '67108864', '347', '67110912', '325');
INSERT INTO `mapstriggers` VALUES ('379', '17049600', '452', '88082201', '396');
INSERT INTO `mapstriggers` VALUES ('380', '17049602', '452', '17049600', '317');
INSERT INTO `mapstriggers` VALUES ('381', '17049604', '423', '17049602', '317');
INSERT INTO `mapstriggers` VALUES ('382', '91753987', '439', '90708227', '178');
INSERT INTO `mapstriggers` VALUES ('383', '91753987', '454', '90708227', '178');
INSERT INTO `mapstriggers` VALUES ('384', '91752963', '472', '91753987', '327');
INSERT INTO `mapstriggers` VALUES ('385', '91752963', '459', '91753987', '327');
INSERT INTO `mapstriggers` VALUES ('386', '91751939', '472', '91752963', '256');
INSERT INTO `mapstriggers` VALUES ('387', '91751939', '459', '91752963', '256');
INSERT INTO `mapstriggers` VALUES ('388', '91750915', '442', '91751939', '269');
INSERT INTO `mapstriggers` VALUES ('389', '91750915', '429', '91751939', '269');
INSERT INTO `mapstriggers` VALUES ('390', '91750915', '439', '95422464', '163');
INSERT INTO `mapstriggers` VALUES ('391', '91750915', '454', '95422464', '163');
INSERT INTO `mapstriggers` VALUES ('392', '91751943', '360', '91752967', '453');
INSERT INTO `mapstriggers` VALUES ('393', '91752967', '466', '91751943', '374');
INSERT INTO `mapstriggers` VALUES ('394', '91752967', '481', '91751943', '374');
INSERT INTO `mapstriggers` VALUES ('395', '91750917', '429', '95424000', '244');
INSERT INTO `mapstriggers` VALUES ('396', '91752961', '443', '95422466', '218');
INSERT INTO `mapstriggers` VALUES ('397', '91752961', '430', '95422466', '218');
INSERT INTO `mapstriggers` VALUES ('398', '91751937', '424', '90704385', '290');
INSERT INTO `mapstriggers` VALUES ('399', '91751941', '442', '90704896', '268');
INSERT INTO `mapstriggers` VALUES ('400', '91751941', '429', '90704896', '268');
INSERT INTO `mapstriggers` VALUES ('401', '91750913', '400', '90706432', '201');
INSERT INTO `mapstriggers` VALUES ('402', '91750913', '386', '90706432', '201');
INSERT INTO `mapstriggers` VALUES ('403', '91753989', '495', '95420416', '346');
INSERT INTO `mapstriggers` VALUES ('404', '91753989', '510', '95420416', '346');
INSERT INTO `mapstriggers` VALUES ('405', '84935177', '382', '73402375', '317');
INSERT INTO `mapstriggers` VALUES ('406', '99355652', '424', '88080898', '415');
INSERT INTO `mapstriggers` VALUES ('407', '99354628', '415', '88080384', '285');
INSERT INTO `mapstriggers` VALUES ('408', '99355648', '429', '88212480', '326');
INSERT INTO `mapstriggers` VALUES ('409', '99354624', '424', '88081408', '274');
INSERT INTO `mapstriggers` VALUES ('410', '91752965', '442', '90708226', '228');
INSERT INTO `mapstriggers` VALUES ('411', '91752965', '429', '90708226', '228');
INSERT INTO `mapstriggers` VALUES ('412', '86248454', '414', '84410882', '412');
INSERT INTO `mapstriggers` VALUES ('413', '86245384', '438', '84411394', '317');
INSERT INTO `mapstriggers` VALUES ('414', '153357316', '411', '153878787', '354');
INSERT INTO `mapstriggers` VALUES ('415', '153355270', '415', '154010371', '341');
INSERT INTO `mapstriggers` VALUES ('416', '54175536', '165', '55053312', '284');
INSERT INTO `mapstriggers` VALUES ('417', '10016', '365', '10016', '397');
INSERT INTO `mapstriggers` VALUES ('418', '11034', '276', '11034', '415');
INSERT INTO `mapstriggers` VALUES ('419', '16163', '310', '16163', '327');
INSERT INTO `mapstriggers` VALUES ('420', '14128', '302', '14128', '397');
