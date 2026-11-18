-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Nov 17, 2026 at 05:48 AM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.0.30

SET SESSION sql_require_primary_key = OFF;
SET FOREIGN_KEY_CHECKS = 0;

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `db_inventory`
--

-- --------------------------------------------------------

--
-- Table structure for table `activitylogs`
--

CREATE TABLE `activitylogs` (
  `Id` bigint(20) NOT NULL,
  `Causer` varchar(150) NOT NULL,
  `Event` varchar(50) NOT NULL,
  `Description` varchar(500) NOT NULL,
  `Subject` varchar(150) NOT NULL,
  `Timestamp` datetime(6) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `activitylogs`
--

INSERT INTO `activitylogs` (`Id`, `Causer`, `Event`, `Description`, `Subject`, `Timestamp`) VALUES
(1, 'Cindy Lestari', 'UPDATE', 'Melakukan aksi UPDATE pada data produk (ID/Ref: 9)', 'PRODUK', '2026-11-15 02:42:45.961586'),
(2, 'Budi Santoso', 'UPDATE', 'Melakukan aksi UPDATE pada data penugasanasets (ID/Ref: 19)', 'PENUGASANASETS', '2026-11-15 09:43:37.184885'),
(3, 'Budi Santoso', 'UPDATE', 'Melakukan aksi UPDATE pada data penugasanasetdetail (ID/Ref: 19)', 'PENUGASANASETDETAIL', '2026-11-15 09:43:37.185026'),
(4, 'Budi Santoso', 'UPDATE', 'Melakukan aksi UPDATE pada data produk_item (ID/Ref: 1)', 'PRODUK_ITEM', '2026-11-15 09:43:37.185037'),
(5, 'Budi Santoso', 'CREATE', 'Melakukan aksi CREATE pada data penugasanasets (ID/Ref: -2147482647)', 'PENUGASANASETS', '2026-11-15 09:44:56.735811'),
(6, 'Budi Santoso', 'CREATE', 'Melakukan aksi CREATE pada data penugasanasetdetail (ID/Ref: -2147482647)', 'PENUGASANASETDETAIL', '2026-11-15 09:44:56.735890'),
(7, 'Budi Santoso', 'UPDATE', 'Melakukan aksi UPDATE pada data produk_item (ID/Ref: 1)', 'PRODUK_ITEM', '2026-11-15 09:44:56.735911'),
(8, 'Budi Santoso', 'UPDATE', 'Melakukan aksi UPDATE pada data penugasanasets (ID/Ref: 20)', 'PENUGASANASETS', '2026-11-15 09:45:57.785960'),
(9, 'Budi Santoso', 'CREATE', 'Melakukan aksi CREATE pada data gudang (ID/Ref: -2147482647)', 'GUDANG', '2026-11-16 11:49:07.014176'),
(10, 'Budi Santoso', 'UPDATE', 'Melakukan aksi UPDATE pada data gudang (ID/Ref: 4)', 'GUDANG', '2026-11-16 11:49:18.171220'),
(11, 'Budi Santoso', 'CREATE', 'Melakukan aksi CREATE pada data rak (ID/Ref: -2147482647)', 'RAK', '2026-11-16 11:50:33.263608'),
(12, 'Budi Santoso', 'UPDATE', 'Melakukan aksi UPDATE pada data rak (ID/Ref: 3)', 'RAK', '2026-11-16 11:50:42.665473'),
(13, 'Budi Santoso', 'CREATE', 'Melakukan aksi CREATE pada data master_uom (ID/Ref: -2147482647)', 'MASTER_UOM', '2026-11-16 11:51:09.364385'),
(14, 'Budi Santoso', 'UPDATE', 'Melakukan aksi UPDATE pada data master_uom (ID/Ref: 22)', 'MASTER_UOM', '2026-11-16 11:51:19.607644'),
(15, 'Budi Santoso', 'DELETE', 'Melakukan aksi DELETE pada data master_uom (ID/Ref: 22)', 'MASTER_UOM', '2026-11-16 11:51:23.181315'),
(16, 'Budi Santoso', 'CREATE', 'Melakukan aksi CREATE pada data kategoris (ID/Ref: -2147482647)', 'KATEGORIS', '2026-11-16 11:51:42.914273'),
(17, 'Budi Santoso', 'UPDATE', 'Melakukan aksi UPDATE pada data kategoris (ID/Ref: 21)', 'KATEGORIS', '2026-11-16 11:51:49.240763'),
(18, 'Budi Santoso', 'DELETE', 'Melakukan aksi DELETE pada data kategoris (ID/Ref: 21)', 'KATEGORIS', '2026-11-16 11:51:52.957374'),
(19, 'Budi Santoso', 'CREATE', 'Melakukan aksi CREATE pada data produk (ID/Ref: -2147482647)', 'PRODUK', '2026-11-16 11:53:09.268701'),
(20, 'Budi Santoso', 'UPDATE', 'Melakukan aksi UPDATE pada data produk (ID/Ref: 10)', 'PRODUK', '2026-11-16 11:53:16.690792'),
(21, 'Budi Santoso', 'CREATE', 'Melakukan aksi CREATE pada data suppliers (ID/Ref: -2147482647)', 'SUPPLIERS', '2026-11-16 11:54:09.505692'),
(22, 'Budi Santoso', 'UPDATE', 'Melakukan aksi UPDATE pada data suppliers (ID/Ref: 21)', 'SUPPLIERS', '2026-11-16 11:54:14.027638'),
(23, 'Budi Santoso', 'DELETE', 'Melakukan aksi DELETE pada data suppliers (ID/Ref: 21)', 'SUPPLIERS', '2026-11-16 11:54:18.063934'),
(24, 'Budi Santoso', 'CREATE', 'Melakukan aksi CREATE pada data karyawans (ID/Ref: -2147482647)', 'KARYAWANS', '2026-11-16 11:55:29.205368'),
(25, 'Budi Santoso', 'UPDATE', 'Melakukan aksi UPDATE pada data karyawans (ID/Ref: 51)', 'KARYAWANS', '2026-11-16 11:55:34.323006'),
(26, 'Budi Santoso', 'DELETE', 'Melakukan aksi DELETE pada data karyawans (ID/Ref: 51)', 'KARYAWANS', '2026-11-16 11:55:39.185737'),
(27, 'Budi Santoso', 'CREATE', 'Melakukan aksi CREATE pada data departments (ID/Ref: -2147482647)', 'DEPARTMENTS', '2026-11-16 11:57:10.413760'),
(28, 'Budi Santoso', 'UPDATE', 'Melakukan aksi UPDATE pada data departments (ID/Ref: 23)', 'DEPARTMENTS', '2026-11-16 11:57:13.612149'),
(29, 'Budi Santoso', 'DELETE', 'Melakukan aksi DELETE pada data departments (ID/Ref: 23)', 'DEPARTMENTS', '2026-11-16 11:57:16.286680'),
(30, 'Budi Santoso', 'CREATE', 'Melakukan aksi CREATE pada data karyawans (ID/Ref: -2147482647)', 'KARYAWANS', '2026-11-16 16:58:04.641446'),
(31, 'Budi Santoso', 'UPDATE', 'Melakukan aksi UPDATE pada data karyawans (ID/Ref: 52)', 'KARYAWANS', '2026-11-16 16:58:10.709468'),
(32, 'Budi Santoso', 'DELETE', 'Melakukan aksi DELETE pada data karyawans (ID/Ref: 52)', 'KARYAWANS', '2026-11-16 16:58:14.784644'),
(33, 'Budi Santoso', 'CREATE', 'Melakukan aksi CREATE pada data departments (ID/Ref: -2147482647)', 'DEPARTMENTS', '2026-11-16 16:58:39.873859'),
(34, 'Budi Santoso', 'UPDATE', 'Melakukan aksi UPDATE pada data departments (ID/Ref: 24)', 'DEPARTMENTS', '2026-11-16 16:58:43.374419'),
(35, 'Budi Santoso', 'DELETE', 'Melakukan aksi DELETE pada data departments (ID/Ref: 24)', 'DEPARTMENTS', '2026-11-16 16:58:45.817663'),
(36, 'Budi Santoso', 'CREATE', 'Melakukan aksi CREATE pada data customers (ID/Ref: -2147482647)', 'CUSTOMERS', '2026-11-16 16:59:22.517478'),
(37, 'Budi Santoso', 'UPDATE', 'Melakukan aksi UPDATE pada data customers (ID/Ref: 21)', 'CUSTOMERS', '2026-11-16 16:59:27.666241'),
(38, 'Budi Santoso', 'DELETE', 'Melakukan aksi DELETE pada data customers (ID/Ref: 21)', 'CUSTOMERS', '2026-11-16 16:59:31.451281'),
(39, 'Budi Santoso', 'CREATE', 'Melakukan aksi CREATE pada data penugasanasets (ID/Ref: -2147482647)', 'PENUGASANASETS', '2026-11-16 17:00:18.968152'),
(40, 'Budi Santoso', 'CREATE', 'Melakukan aksi CREATE pada data penugasanasetdetail (ID/Ref: -2147482647)', 'PENUGASANASETDETAIL', '2026-11-16 17:00:18.968171'),
(41, 'Budi Santoso', 'UPDATE', 'Melakukan aksi UPDATE pada data penugasanasets (ID/Ref: 21)', 'PENUGASANASETS', '2026-11-16 17:01:22.702195'),
(42, 'Budi Santoso', 'UPDATE', 'Melakukan aksi UPDATE pada data penugasanasets (ID/Ref: 21)', 'PENUGASANASETS', '2026-11-16 17:02:11.730124'),
(43, 'Budi Santoso', 'UPDATE', 'Melakukan aksi UPDATE pada data penugasanasetdetail (ID/Ref: 21)', 'PENUGASANASETDETAIL', '2026-11-16 17:02:11.730133'),
(44, 'Budi Santoso', 'UPDATE', 'Melakukan aksi UPDATE pada data produk (ID/Ref: 10)', 'PRODUK', '2026-11-16 17:02:11.730148'),
(45, 'Budi Santoso', 'CREATE', 'Melakukan aksi CREATE pada data penugasanasets (ID/Ref: -2147482646)', 'PENUGASANASETS', '2026-11-16 17:24:45.101402'),
(46, 'Budi Santoso', 'CREATE', 'Melakukan aksi CREATE pada data penugasanasetdetail (ID/Ref: -2147482646)', 'PENUGASANASETDETAIL', '2026-11-16 17:24:45.101496'),
(47, 'Budi Santoso', 'UPDATE', 'Melakukan aksi UPDATE pada data produk (ID/Ref: 10)', 'PRODUK', '2026-11-16 17:24:45.101515'),
(48, 'Budi Santoso', 'UPDATE', 'Melakukan aksi UPDATE pada data penugasanasets (ID/Ref: 22)', 'PENUGASANASETS', '2026-11-16 17:25:11.790646');

-- --------------------------------------------------------

--
-- Table structure for table `aspnetroleclaims`
--

CREATE TABLE `aspnetroleclaims` (
  `Id` int(11) NOT NULL,
  `RoleId` varchar(255) NOT NULL,
  `ClaimType` longtext DEFAULT NULL,
  `ClaimValue` longtext DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `aspnetroleclaims`
--

INSERT INTO `aspnetroleclaims` (`Id`, `RoleId`, `ClaimType`, `ClaimValue`) VALUES
(1, '53ca8105-6b46-4eee-9981-4ff3b7529f30', 'Permission', 'view dashboard'),
(2, '53ca8105-6b46-4eee-9981-4ff3b7529f30', 'Permission', 'view stock matrix'),
(3, '53ca8105-6b46-4eee-9981-4ff3b7529f30', 'Permission', 'view stock mutations'),
(4, '53ca8105-6b46-4eee-9981-4ff3b7529f30', 'Permission', 'manage users'),
(5, '53ca8105-6b46-4eee-9981-4ff3b7529f30', 'Permission', 'manage settings'),
(6, '53ca8105-6b46-4eee-9981-4ff3b7529f30', 'Permission', 'view logs'),
(7, '53ca8105-6b46-4eee-9981-4ff3b7529f30', 'Permission', 'manage warehouses'),
(8, '53ca8105-6b46-4eee-9981-4ff3b7529f30', 'Permission', 'manage suppliers'),
(9, '53ca8105-6b46-4eee-9981-4ff3b7529f30', 'Permission', 'manage transfer orders'),
(10, '53ca8105-6b46-4eee-9981-4ff3b7529f30', 'Permission', 'manage employees'),
(11, '53ca8105-6b46-4eee-9981-4ff3b7529f30', 'Permission', 'manage loans'),
(12, '53ca8105-6b46-4eee-9981-4ff3b7529f30', 'Permission', 'manage assignments'),
(13, '53ca8105-6b46-4eee-9981-4ff3b7529f30', 'Permission', 'manage departments'),
(14, '53ca8105-6b46-4eee-9981-4ff3b7529f30', 'Permission', 'manage stock opname'),
(15, '53ca8105-6b46-4eee-9981-4ff3b7529f30', 'Permission', 'manage products'),
(16, '8c0e7ef7-236b-4004-9a9c-80d5aa1675a8', 'Permission', 'view dashboard'),
(17, '8c0e7ef7-236b-4004-9a9c-80d5aa1675a8', 'Permission', 'view stock matrix'),
(18, '8c0e7ef7-236b-4004-9a9c-80d5aa1675a8', 'Permission', 'view stock mutations'),
(19, '8c0e7ef7-236b-4004-9a9c-80d5aa1675a8', 'Permission', 'view logs'),
(20, '8c0e7ef7-236b-4004-9a9c-80d5aa1675a8', 'Permission', 'manage warehouses'),
(21, '8c0e7ef7-236b-4004-9a9c-80d5aa1675a8', 'Permission', 'manage products'),
(22, '8c0e7ef7-236b-4004-9a9c-80d5aa1675a8', 'Permission', 'manage suppliers'),
(23, '8c0e7ef7-236b-4004-9a9c-80d5aa1675a8', 'Permission', 'manage transfer orders'),
(24, '8c0e7ef7-236b-4004-9a9c-80d5aa1675a8', 'Permission', 'manage employees'),
(25, 'fa38c984-0ca9-40b4-8de1-5116b232e0f7', 'Permission', 'view dashboard'),
(26, 'fa38c984-0ca9-40b4-8de1-5116b232e0f7', 'Permission', 'view stock matrix'),
(27, 'fa38c984-0ca9-40b4-8de1-5116b232e0f7', 'Permission', 'view stock mutations'),
(28, 'fa38c984-0ca9-40b4-8de1-5116b232e0f7', 'Permission', 'manage users'),
(29, '80085233-87ab-49a1-b420-8d57b65f5e59', 'Permission', 'manage users'),
(30, '80085233-87ab-49a1-b420-8d57b65f5e59', 'Permission', 'manage settings'),
(31, '80085233-87ab-49a1-b420-8d57b65f5e59', 'Permission', 'view logs'),
(32, '212e9aa8-fd35-467a-8165-b789582fe471', 'Permission', 'view dashboard'),
(33, '212e9aa8-fd35-467a-8165-b789582fe471', 'Permission', 'view stock matrix'),
(34, '212e9aa8-fd35-467a-8165-b789582fe471', 'Permission', 'view stock mutations'),
(35, '212e9aa8-fd35-467a-8165-b789582fe471', 'Permission', 'manage warehouses'),
(36, '212e9aa8-fd35-467a-8165-b789582fe471', 'Permission', 'manage products'),
(37, 'db442a01-3da5-471a-b6fb-44c47346902f', 'Permission', 'view dashboard'),
(52, 'db442a01-3da5-471a-b6fb-44c47346902f', 'Permission', 'view user guide'),
(53, 'db442a01-3da5-471a-b6fb-44c47346902f', 'Permission', 'manage assignments'),
(54, 'db442a01-3da5-471a-b6fb-44c47346902f', 'Permission', 'manage loans'),
(55, 'db442a01-3da5-471a-b6fb-44c47346902f', 'Permission', 'view stock mutations'),
(56, 'db442a01-3da5-471a-b6fb-44c47346902f', 'Permission', 'view stock matrix'),
(57, 'af74d88d-ccb0-4b55-b98b-142fea17d95c', 'Permission', 'view dashboard'),
(58, 'af74d88d-ccb0-4b55-b98b-142fea17d95c', 'Permission', 'view stock matrix'),
(59, 'af74d88d-ccb0-4b55-b98b-142fea17d95c', 'Permission', 'view user guide'),
(60, 'af74d88d-ccb0-4b55-b98b-142fea17d95c', 'Permission', 'view stock mutations'),
(61, 'af74d88d-ccb0-4b55-b98b-142fea17d95c', 'Permission', 'manage loans'),
(62, 'af74d88d-ccb0-4b55-b98b-142fea17d95c', 'Permission', 'manage stock opname'),
(63, 'af74d88d-ccb0-4b55-b98b-142fea17d95c', 'Permission', 'manage assignments'),
(64, 'af74d88d-ccb0-4b55-b98b-142fea17d95c', 'Permission', 'view reparasi asset'),
(65, 'af74d88d-ccb0-4b55-b98b-142fea17d95c', 'Permission', 'manage transfer orders'),
(66, 'af74d88d-ccb0-4b55-b98b-142fea17d95c', 'Permission', 'manage products'),
(67, 'af74d88d-ccb0-4b55-b98b-142fea17d95c', 'Permission', 'manage suppliers'),
(68, 'af74d88d-ccb0-4b55-b98b-142fea17d95c', 'Permission', 'manage warehouses'),
(69, '4e3b18ad-18f7-415e-9ef9-3349dff3efe1', 'Permission', 'view dashboard'),
(70, '4e3b18ad-18f7-415e-9ef9-3349dff3efe1', 'Permission', 'view stock matrix'),
(71, '4e3b18ad-18f7-415e-9ef9-3349dff3efe1', 'Permission', 'view user guide'),
(72, '4e3b18ad-18f7-415e-9ef9-3349dff3efe1', 'Permission', 'view stock mutations'),
(73, '4e3b18ad-18f7-415e-9ef9-3349dff3efe1', 'Permission', 'manage warehouses'),
(74, '4e3b18ad-18f7-415e-9ef9-3349dff3efe1', 'Permission', 'manage stock opname'),
(75, '4e3b18ad-18f7-415e-9ef9-3349dff3efe1', 'Permission', 'manage assignments'),
(76, '4e3b18ad-18f7-415e-9ef9-3349dff3efe1', 'Permission', 'view reparasi asset'),
(77, '4e3b18ad-18f7-415e-9ef9-3349dff3efe1', 'Permission', 'manage loans'),
(78, '4e3b18ad-18f7-415e-9ef9-3349dff3efe1', 'Permission', 'manage transfer orders'),
(79, '528be043-74b1-4c1d-8791-c330ec9cc421', 'Permission', 'view dashboard'),
(80, '528be043-74b1-4c1d-8791-c330ec9cc421', 'Permission', 'view stock mutations'),
(82, '528be043-74b1-4c1d-8791-c330ec9cc421', 'Permission', 'view user guide'),
(83, '528be043-74b1-4c1d-8791-c330ec9cc421', 'Permission', 'view stock matrix'),
(84, '528be043-74b1-4c1d-8791-c330ec9cc421', 'Permission', 'view logs'),
(85, '528be043-74b1-4c1d-8791-c330ec9cc421', 'Permission', 'manage warehouses'),
(86, '528be043-74b1-4c1d-8791-c330ec9cc421', 'Permission', 'manage suppliers'),
(87, '528be043-74b1-4c1d-8791-c330ec9cc421', 'Permission', 'manage transfer orders'),
(88, '528be043-74b1-4c1d-8791-c330ec9cc421', 'Permission', 'manage employees'),
(89, '528be043-74b1-4c1d-8791-c330ec9cc421', 'Permission', 'manage loans'),
(90, '528be043-74b1-4c1d-8791-c330ec9cc421', 'Permission', 'view reparasi asset'),
(91, '528be043-74b1-4c1d-8791-c330ec9cc421', 'Permission', 'manage assignments'),
(92, '528be043-74b1-4c1d-8791-c330ec9cc421', 'Permission', 'manage departments'),
(93, '528be043-74b1-4c1d-8791-c330ec9cc421', 'Permission', 'manage stock opname'),
(94, '528be043-74b1-4c1d-8791-c330ec9cc421', 'Permission', 'manage products'),
(95, '3f27ffeb-2740-4db6-8fd4-4986497b7160', 'Permission', 'view dashboard'),
(96, '3f27ffeb-2740-4db6-8fd4-4986497b7160', 'Permission', 'view stock matrix'),
(97, '3f27ffeb-2740-4db6-8fd4-4986497b7160', 'Permission', 'view user guide'),
(98, '3f27ffeb-2740-4db6-8fd4-4986497b7160', 'Permission', 'view stock mutations'),
(99, '3f27ffeb-2740-4db6-8fd4-4986497b7160', 'Permission', 'view logs'),
(100, '3f27ffeb-2740-4db6-8fd4-4986497b7160', 'Permission', 'manage warehouses'),
(101, '3f27ffeb-2740-4db6-8fd4-4986497b7160', 'Permission', 'manage suppliers'),
(102, '3f27ffeb-2740-4db6-8fd4-4986497b7160', 'Permission', 'manage transfer orders'),
(103, '3f27ffeb-2740-4db6-8fd4-4986497b7160', 'Permission', 'manage employees'),
(104, '3f27ffeb-2740-4db6-8fd4-4986497b7160', 'Permission', 'manage loans'),
(105, '3f27ffeb-2740-4db6-8fd4-4986497b7160', 'Permission', 'view reparasi asset'),
(106, '3f27ffeb-2740-4db6-8fd4-4986497b7160', 'Permission', 'manage assignments'),
(107, '3f27ffeb-2740-4db6-8fd4-4986497b7160', 'Permission', 'manage departments'),
(108, '3f27ffeb-2740-4db6-8fd4-4986497b7160', 'Permission', 'manage stock opname'),
(109, '3f27ffeb-2740-4db6-8fd4-4986497b7160', 'Permission', 'manage products'),
(110, 'af74d88d-ccb0-4b55-b98b-142fea17d95c', 'Permission', 'view stock cards'),
(111, '528be043-74b1-4c1d-8791-c330ec9cc421', 'Permission', 'view stock cards'),
(112, '528be043-74b1-4c1d-8791-c330ec9cc421', 'Permission', 'manage reparasi aset'),
(113, '528be043-74b1-4c1d-8791-c330ec9cc421', 'Permission', 'manage uom'),
(114, '528be043-74b1-4c1d-8791-c330ec9cc421', 'Permission', 'view asset reports'),
(115, '528be043-74b1-4c1d-8791-c330ec9cc421', 'Permission', 'manage customer'),
(116, '528be043-74b1-4c1d-8791-c330ec9cc421', 'Permission', 'manage stock transactions'),
(117, '528be043-74b1-4c1d-8791-c330ec9cc421', 'Permission', 'manage racks'),
(118, '528be043-74b1-4c1d-8791-c330ec9cc421', 'Permission', 'view stock in/out reports'),
(119, '528be043-74b1-4c1d-8791-c330ec9cc421', 'Permission', 'manage categories'),
(120, '4e3b18ad-18f7-415e-9ef9-3349dff3efe1', 'Permission', 'view stock cards'),
(121, '4e3b18ad-18f7-415e-9ef9-3349dff3efe1', 'Permission', 'manage products'),
(122, '4e3b18ad-18f7-415e-9ef9-3349dff3efe1', 'Permission', 'manage departments'),
(123, '4e3b18ad-18f7-415e-9ef9-3349dff3efe1', 'Permission', 'manage suppliers'),
(124, '4e3b18ad-18f7-415e-9ef9-3349dff3efe1', 'Permission', 'manage employees'),
(125, '4e3b18ad-18f7-415e-9ef9-3349dff3efe1', 'Permission', 'manage customer'),
(126, '4e3b18ad-18f7-415e-9ef9-3349dff3efe1', 'Permission', 'view asset reports'),
(127, '4e3b18ad-18f7-415e-9ef9-3349dff3efe1', 'Permission', 'manage uom'),
(128, '4e3b18ad-18f7-415e-9ef9-3349dff3efe1', 'Permission', 'manage reparasi aset'),
(129, '4e3b18ad-18f7-415e-9ef9-3349dff3efe1', 'Permission', 'manage stock transactions'),
(130, '4e3b18ad-18f7-415e-9ef9-3349dff3efe1', 'Permission', 'manage racks'),
(131, '4e3b18ad-18f7-415e-9ef9-3349dff3efe1', 'Permission', 'view stock in/out reports'),
(132, '4e3b18ad-18f7-415e-9ef9-3349dff3efe1', 'Permission', 'manage categories'),
(133, '3f27ffeb-2740-4db6-8fd4-4986497b7160', 'Permission', 'view stock cards'),
(134, '3f27ffeb-2740-4db6-8fd4-4986497b7160', 'Permission', 'manage customer'),
(135, '3f27ffeb-2740-4db6-8fd4-4986497b7160', 'Permission', 'view asset reports'),
(136, '3f27ffeb-2740-4db6-8fd4-4986497b7160', 'Permission', 'manage uom'),
(137, '3f27ffeb-2740-4db6-8fd4-4986497b7160', 'Permission', 'manage reparasi aset'),
(138, '3f27ffeb-2740-4db6-8fd4-4986497b7160', 'Permission', 'manage stock transactions'),
(139, '3f27ffeb-2740-4db6-8fd4-4986497b7160', 'Permission', 'manage racks'),
(140, '3f27ffeb-2740-4db6-8fd4-4986497b7160', 'Permission', 'view stock in/out reports'),
(141, '3f27ffeb-2740-4db6-8fd4-4986497b7160', 'Permission', 'manage categories'),
(142, 'db442a01-3da5-471a-b6fb-44c47346902f', 'Permission', 'view stock cards'),
(143, 'db442a01-3da5-471a-b6fb-44c47346902f', 'Permission', 'manage warehouses'),
(144, 'db442a01-3da5-471a-b6fb-44c47346902f', 'Permission', 'manage products'),
(145, 'db442a01-3da5-471a-b6fb-44c47346902f', 'Permission', 'manage stock opname'),
(146, 'db442a01-3da5-471a-b6fb-44c47346902f', 'Permission', 'manage departments'),
(147, 'db442a01-3da5-471a-b6fb-44c47346902f', 'Permission', 'manage suppliers'),
(148, 'db442a01-3da5-471a-b6fb-44c47346902f', 'Permission', 'manage transfer orders'),
(149, 'db442a01-3da5-471a-b6fb-44c47346902f', 'Permission', 'manage employees'),
(150, 'db442a01-3da5-471a-b6fb-44c47346902f', 'Permission', 'view reparasi asset'),
(151, 'db442a01-3da5-471a-b6fb-44c47346902f', 'Permission', 'manage categories'),
(152, 'db442a01-3da5-471a-b6fb-44c47346902f', 'Permission', 'view stock in/out reports'),
(153, 'db442a01-3da5-471a-b6fb-44c47346902f', 'Permission', 'manage racks'),
(154, 'db442a01-3da5-471a-b6fb-44c47346902f', 'Permission', 'manage stock transactions'),
(155, 'db442a01-3da5-471a-b6fb-44c47346902f', 'Permission', 'manage reparasi aset'),
(156, 'db442a01-3da5-471a-b6fb-44c47346902f', 'Permission', 'manage uom'),
(157, 'db442a01-3da5-471a-b6fb-44c47346902f', 'Permission', 'view asset reports'),
(158, 'db442a01-3da5-471a-b6fb-44c47346902f', 'Permission', 'manage customer'),
(159, 'af74d88d-ccb0-4b55-b98b-142fea17d95c', 'Permission', 'view stock in/out reports'),
(160, 'af74d88d-ccb0-4b55-b98b-142fea17d95c', 'Permission', 'manage stock transactions'),
(161, 'af74d88d-ccb0-4b55-b98b-142fea17d95c', 'Permission', 'manage racks'),
(162, 'af74d88d-ccb0-4b55-b98b-142fea17d95c', 'Permission', 'manage uom'),
(163, 'af74d88d-ccb0-4b55-b98b-142fea17d95c', 'Permission', 'manage reparasi aset'),
(164, 'af74d88d-ccb0-4b55-b98b-142fea17d95c', 'Permission', 'view asset reports'),
(165, 'af74d88d-ccb0-4b55-b98b-142fea17d95c', 'Permission', 'manage customer'),
(166, 'af74d88d-ccb0-4b55-b98b-142fea17d95c', 'Permission', 'manage categories');

-- --------------------------------------------------------

--
-- Table structure for table `aspnetroles`
--

CREATE TABLE `aspnetroles` (
  `Id` varchar(255) NOT NULL,
  `Name` varchar(256) DEFAULT NULL,
  `NormalizedName` varchar(256) DEFAULT NULL,
  `ConcurrencyStamp` longtext DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `aspnetroles`
--

INSERT INTO `aspnetroles` (`Id`, `Name`, `NormalizedName`, `ConcurrencyStamp`) VALUES
('0b1198d7-f735-40c7-97b2-29e017c27865', 'Super Admin', 'SUPER ADMIN', NULL),
('3f27ffeb-2740-4db6-8fd4-4986497b7160', 'Manager', 'MANAGER', '592debbc-74f3-4282-8cb1-3b765aaaf113'),
('4e3b18ad-18f7-415e-9ef9-3349dff3efe1', 'User', 'USER', 'a858ce25-6652-4874-9239-32cc750f76d0'),
('528be043-74b1-4c1d-8791-c330ec9cc421', 'Warehouse Manager', 'WAREHOUSE MANAGER', '61af62b9-fc60-4500-9ab3-6d333a5a0a08'),
('af74d88d-ccb0-4b55-b98b-142fea17d95c', 'Warehouse Staff', 'WAREHOUSE STAFF', '715b7ade-1b7e-40df-ac2d-63879c13eab2'),
('db442a01-3da5-471a-b6fb-44c47346902f', 'Admin', 'ADMIN', '873e4471-cbc2-43af-8e59-9347e252247f');

-- --------------------------------------------------------

--
-- Table structure for table `aspnetuserclaims`
--

CREATE TABLE `aspnetuserclaims` (
  `Id` int(11) NOT NULL,
  `UserId` varchar(255) NOT NULL,
  `ClaimType` longtext DEFAULT NULL,
  `ClaimValue` longtext DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `aspnetuserlogins`
--

CREATE TABLE `aspnetuserlogins` (
  `LoginProvider` varchar(255) NOT NULL,
  `ProviderKey` varchar(255) NOT NULL,
  `ProviderDisplayName` longtext DEFAULT NULL,
  `UserId` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `aspnetuserroles`
--

CREATE TABLE `aspnetuserroles` (
  `UserId` varchar(255) NOT NULL,
  `RoleId` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `aspnetuserroles`
--

INSERT INTO `aspnetuserroles` (`UserId`, `RoleId`) VALUES
('159d7a21-12f1-4fe8-b1ef-2319890fa177', '528be043-74b1-4c1d-8791-c330ec9cc421'),
('6ad23b28-3250-4748-9c9a-d2fcb0d0c926', 'af74d88d-ccb0-4b55-b98b-142fea17d95c'),
('85425eb2-9383-4c86-9375-b9941bdc0569', '4e3b18ad-18f7-415e-9ef9-3349dff3efe1'),
('a75255c2-f777-4600-b342-0af8bd3c9d42', '3f27ffeb-2740-4db6-8fd4-4986497b7160'),
('b45dcd36-c843-4bcd-9721-8aa70525b76c', '0b1198d7-f735-40c7-97b2-29e017c27865'),
('c9fc1019-24a2-4451-9041-da0661774136', 'af74d88d-ccb0-4b55-b98b-142fea17d95c'),
('cf78b8fa-3c9b-48df-b6fc-194e5dcce568', 'db442a01-3da5-471a-b6fb-44c47346902f'),
('ef12b9bc-2c23-477f-bc2f-10f15df0477e', 'af74d88d-ccb0-4b55-b98b-142fea17d95c');

-- --------------------------------------------------------

--
-- Table structure for table `aspnetusers`
--

CREATE TABLE `aspnetusers` (
  `Id` varchar(255) NOT NULL,
  `UserName` varchar(256) DEFAULT NULL,
  `NormalizedUserName` varchar(256) DEFAULT NULL,
  `Email` varchar(256) DEFAULT NULL,
  `NormalizedEmail` varchar(256) DEFAULT NULL,
  `EmailConfirmed` tinyint(1) NOT NULL,
  `PasswordHash` longtext DEFAULT NULL,
  `SecurityStamp` longtext DEFAULT NULL,
  `ConcurrencyStamp` longtext DEFAULT NULL,
  `PhoneNumber` longtext DEFAULT NULL,
  `PhoneNumberConfirmed` tinyint(1) NOT NULL,
  `TwoFactorEnabled` tinyint(1) NOT NULL,
  `LockoutEnd` datetime(6) DEFAULT NULL,
  `LockoutEnabled` tinyint(1) NOT NULL,
  `AccessFailedCount` int(11) NOT NULL,
  `CreatedAt` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  `FullName` longtext NOT NULL,
  `KaryawanId` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `aspnetusers`
--

INSERT INTO `aspnetusers` (`Id`, `UserName`, `NormalizedUserName`, `Email`, `NormalizedEmail`, `EmailConfirmed`, `PasswordHash`, `SecurityStamp`, `ConcurrencyStamp`, `PhoneNumber`, `PhoneNumberConfirmed`, `TwoFactorEnabled`, `LockoutEnd`, `LockoutEnabled`, `AccessFailedCount`, `CreatedAt`, `FullName`, `KaryawanId`) VALUES
('159d7a21-12f1-4fe8-b1ef-2319890fa177', 'warehousemanager@example.com', 'WAREHOUSEMANAGER@EXAMPLE.COM', 'warehousemanager@example.com', 'WAREHOUSEMANAGER@EXAMPLE.COM', 1, 'AQAAAAIAAYagAAAAEDPVrg44CvK74kMk/xSnN5gE0yJVUfC5NeVRrgiy3OHV8eWBsackWkSeljWu9h9ghw==', 'C3ZMSEGCNTHKDDEEVIMFF3PYNKLXFQ5R', '0110d886-6b50-4849-9a67-802174a3d373', NULL, 0, 0, NULL, 1, 0, '2026-11-08 15:30:35.097696', '', 1),
('6ad23b28-3250-4748-9c9a-d2fcb0d0c926', 'warehousestaff@example.com', 'WAREHOUSESTAFF@EXAMPLE.COM', 'warehousestaff@example.com', 'WAREHOUSESTAFF@EXAMPLE.COM', 1, 'AQAAAAIAAYagAAAAEMVTMzFh0ZjJ+x62imkvzKhMlbg417ivK7W4oeG+7mYhieAEkMrnfAm40rqeSfPOng==', 'EAKYOQ7CFJFIVN6ZKZHXMPRZT3JYMLO3', '97366d1b-8862-4b14-b651-37a13a28873c', NULL, 0, 0, NULL, 1, 0, '2026-11-08 15:30:35.398926', '', NULL),
('85425eb2-9383-4c86-9375-b9941bdc0569', 'user@example.com', 'USER@EXAMPLE.COM', 'user@example.com', 'USER@EXAMPLE.COM', 1, 'AQAAAAIAAYagAAAAEJ88oBANF+kYPsCHXaRZ9gX3DpxtQZNNyt9iGl39ENL+DB/dlcIv/5b1Gwt3kYXUug==', 'SSIP2B4GW6W2TSPHY3LSB6BLIRQ6P63E', '5aa0d564-64ad-49d3-ac71-f7104efa18d6', NULL, 0, 0, NULL, 1, 0, '2026-11-08 15:30:35.627029', '', NULL),
('a75255c2-f777-4600-b342-0af8bd3c9d42', 'manager@example.com', 'MANAGER@EXAMPLE.COM', 'manager@example.com', 'MANAGER@EXAMPLE.COM', 1, 'AQAAAAIAAYagAAAAENpnkDAhD9gVLeTVcDDr8gTRyDt6O4XtRmmxAQeWpg88AnK4fRIGfI5yalwFKdfvcA==', 'HFCHZBRDX4OSVXF4AWS4KKRCMCZXGA27', 'cb24fab1-5448-4d84-98fd-d52e53dfb1f4', NULL, 0, 0, NULL, 1, 0, '2026-11-08 15:30:34.539602', '', 1),
('b45dcd36-c843-4bcd-9721-8aa70525b76c', 'superadmin@example.com', 'SUPERADMIN@EXAMPLE.COM', 'superadmin@example.com', 'SUPERADMIN@EXAMPLE.COM', 1, 'AQAAAAIAAYagAAAAEGjUQMYE3hBo5ubCejTSmD9O3BwiRPE7KTrop4kL5BZI2BltzcE9ZZqf9sQOnRvvTA==', '7S52Z7JZPMIH6PGYTRJPZOIZ65E7EB7O', '67cf0f8a-b50a-4d16-a9b8-edf90bbc4955', NULL, 0, 0, NULL, 1, 0, '2026-11-08 15:30:33.595863', '', 1),
('c9fc1019-24a2-4451-9041-da0661774136', 'budi@example.com', 'BUDI@EXAMPLE.COM', 'budi@example.com', 'BUDI@EXAMPLE.COM', 1, 'AQAAAAIAAYagAAAAEI0HqYLjodazSHfy7J5VE9VGskbgk9bNacmTdeSfH/RdtGUV04aAIMSXGPW/BFyQfQ==', '5TPGP3IVSVHT6PIUQQHTCGWTOOVO3KSN', 'd681f32a-d75a-4a5b-9acd-e05b6ef98c83', NULL, 0, 0, NULL, 1, 0, '2026-11-13 09:49:29.987641', 'budi', NULL),
('cf78b8fa-3c9b-48df-b6fc-194e5dcce568', 'admin@example.com', 'ADMIN@EXAMPLE.COM', 'admin@example.com', 'ADMIN@EXAMPLE.COM', 1, 'AQAAAAIAAYagAAAAEAML0b4emEtN/PK/Zog3BVxoX91uf7n/Schht+iNkJIC3DASTHIvpWvytfquTZY9FA==', 'PVYJJAJ6AMK4MEZURCUWP4XPOIOQW53M', '376eb752-3df0-4227-acb7-f168bfb2eb1f', NULL, 0, 0, NULL, 1, 0, '2026-11-08 15:30:34.270110', '', NULL),
('ef12b9bc-2c23-477f-bc2f-10f15df0477e', 'cindy@example.com', 'CINDY@EXAMPLE.COM', 'cindy@example.com', 'CINDY@EXAMPLE.COM', 1, 'AQAAAAIAAYagAAAAEN3BG9T4wTl42rJPIeJCwKhFnEvm3l1lGHlEFzFnfT29UrVRmp9k1LkCZDEYIiaLnw==', 'XBCZEPVHGRWWAOESPMSJZVM3LHJZPZ67', 'a5da867e-1dda-49b2-b9f4-317939603743', NULL, 0, 0, NULL, 1, 0, '2026-11-13 10:03:43.900015', 'Cindy Lestari', 3);

-- --------------------------------------------------------

--
-- Table structure for table `aspnetusertokens`
--

CREATE TABLE `aspnetusertokens` (
  `UserId` varchar(255) NOT NULL,
  `LoginProvider` varchar(255) NOT NULL,
  `Name` varchar(255) NOT NULL,
  `Value` longtext DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `customers`
--

CREATE TABLE `customers` (
  `Id` int(11) NOT NULL,
  `KodeCustomer` varchar(20) NOT NULL,
  `NamaCustomer` varchar(150) NOT NULL,
  `Email` varchar(100) NOT NULL,
  `NoTelepon` varchar(20) NOT NULL,
  `Alamat` varchar(500) NOT NULL,
  `IsActive` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `customers`
--

INSERT INTO `customers` (`Id`, `KodeCustomer`, `NamaCustomer`, `Email`, `NoTelepon`, `Alamat`, `IsActive`) VALUES
(1, 'CUST-001', 'PT Solusi Digital', 'info@solusidigital.com', '021-5551234', 'Gedung Cyber Lt. 5, Kuningan, Jakarta Selatan', 1),
(2, 'CUST-002', 'PT Trans Logistics', 'ops@translog.co.id', '021-8884321', 'Kawasan Industri Jababeka, Cikarang, Bekasi', 1),
(3, 'CUST-003', 'PT Mega Pratama', 'purchasing@megapratama.com', '021-7779876', 'BSD Green Office Park, Tangerang, Banten', 1),
(4, 'CUST-004', 'CV Berkah Sentosa', 'info@berkahsentosa.com', '022-4215678', 'Jl. Asia Afrika No. 12, Bandung', 1),
(5, 'CUST-005', 'PT Mitra Abadi Sejahtera', 'cs@mitraabadi.co.id', '031-5347890', 'Jl. Pemuda No. 45, Surabaya', 1),
(6, 'CUST-006', 'CV Cipta Karya Mandiri', 'admin@ciptakarya.com', '024-8441234', 'Jl. Pandanaran No. 88, Semarang', 1),
(7, 'CUST-007', 'PT Global Media Network', 'support@globalmedia.id', '021-5290876', 'SCBD Sudirman, Jakarta Selatan', 1),
(8, 'CUST-008', 'PT Nusantara Techno', 'contact@nusandratechno.co.id', '031-8293456', 'Jl. Raya Darmo No. 100, Surabaya', 1),
(9, 'CUST-009', 'CV Surya Gemilang', 'suryagemilang@gmail.com', '0274-512789', 'Jl. Malioboro No. 20, Yogyakarta', 1),
(10, 'CUST-010', 'PT Indah Perkasa', 'info@indahperkasa.co.id', '061-4561234', 'Jl. Gatot Subroto No. 50, Medan', 1),
(11, 'CUST-011', 'PT Bina Sarana Digital', 'binsar@digital.co.id', '021-7894561', 'Jl. TB Simatupang No. 15, Jakarta Selatan', 1),
(12, 'CUST-012', 'CV Delta Mandiri', 'deltamandiri@yahoo.com', '0251-8324567', 'Jl. Pajajaran No. 40, Bogor', 1),
(13, 'CUST-013', 'PT Graha Anugerah', 'graha@anugerah.id', '031-5028976', 'Jl. Gubeng Kertajaya No. 5, Surabaya', 1),
(14, 'CUST-014', 'PT Paramount Niaga', 'contact@paramountniaga.com', '021-4201234', 'Jl. Cempaka Putih Raya No. 9, Jakarta Pusat', 1),
(15, 'CUST-015', 'CV Sejahtera Bersama', 'sejahtera@bersama.com', '022-7304567', 'Jl. Buah Batu No. 110, Bandung', 1),
(16, 'CUST-016', 'PT Prima Solusi Utama', 'prima@solusiutama.co.id', '021-5559876', 'Pluit Mega Mall Blok A, Jakarta Utara', 1),
(17, 'CUST-017', 'CV Rajawali Perkasa', 'rajawali@perkasa.com', '031-8412345', 'Jl. HR Muhammad No. 88, Surabaya', 1),
(18, 'CUST-018', 'PT Cyber Data Indonesia', 'info@cyberdata.co.id', '021-5790123', 'Menara Jamsostek Lt. 12, Jakarta Selatan', 1),
(19, 'CUST-019', 'CV Omega Teknik', 'omegateknik@gmail.com', '022-6014321', 'Jl. Pasirkaliki No. 75, Bandung', 0),
(20, 'CUST-020', 'PT Apex Global Solusi', 'apex@globalsolusi.co.id', '021-2998877', 'Grand Indonesia West Mall, Jakarta Pusat', 1),
(21, 'CUST-021', 'PT Sinergi Data Nusantara', 'info@sinergidata.co.id', '021-5550192', 'Jl. HR Rasuna Said Blok X-2, Jakarta Selatan', 1),
(22, 'CUST-022', 'CV Berkah Mandiri Perkasa', 'berkahmandiri@gmail.com', '022-8730123', 'Jl. Soekarno Hatta No. 205, Bandung', 1),
(23, 'CUST-023', 'PT Buana Raya Logistik', 'ops@buanaraya.co.id', '031-8419988', 'Jl. Rungkut Industri Raya No. 18, Surabaya', 1),
(24, 'CUST-024', 'PT Delta Nusa Solusi', 'support@deltanusa.id', '024-8412390', 'Jl. Pandanaran No. 45, Semarang', 1),
(25, 'CUST-025', 'CV Mitra Sejati Teknik', 'mitrasejati@yahoo.com', '0341-356789', 'Jl. Borobudur No. 22, Malang', 1),
(26, 'CUST-026', 'PT Cyber Karya Indonesia', 'admin@cyberkarya.co.id', '021-5799881', 'Menara Batavia Lt. 15, Jakarta Pusat', 1),
(27, 'CUST-027', 'PT Mega Central Niaga', 'contact@megacentral.com', '061-4512389', 'Jl. Imam Bonjol No. 78, Medan', 1),
(28, 'CUST-028', 'CV Anugerah Jaya Abadi', 'anugerahjaya@gmail.com', '0274-589012', 'Jl. Solo Km. 9, Yogyakarta', 1),
(29, 'CUST-029', 'PT Nusantara Global Media', 'info@nusantaraglobal.id', '031-5021144', 'Jl. Gubeng Kertajaya No. 25, Surabaya', 1),
(30, 'CUST-030', 'PT Primajaya Solusindo', 'halo@primajaya.co.id', '021-4209988', 'Jl. Letjen Suprapto No. 60, Jakarta Pusat', 1),
(31, 'CUST-031', 'CV Surya Mandiri Teknik', 'suryamandiri@gmail.com', '022-6031122', 'Jl. Rajawali No. 12, Bandung', 1),
(32, 'CUST-032', 'PT Graha Informatika', 'grahainfo@corporation.id', '021-7889900', 'Jl. TB Simatupang No. 45, Jakarta Selatan', 1),
(33, 'CUST-033', 'PT Tirta Kencana Mulia', 'tirtakencana@yahoo.com', '0251-8314455', 'Jl. Ir. H. Juanda No. 10, Bogor', 1),
(34, 'CUST-034', 'CV Cipta Sarana Mandiri', 'ciptasarana@gmail.com', '031-8294433', 'Jl. Jemursari No. 120, Surabaya', 1),
(35, 'CUST-035', 'PT Indobuild Sarana Utama', 'indobuild@utama.co.id', '021-5554321', 'Pluit Kencana No. 8, Jakarta Utara', 1),
(36, 'CUST-036', 'PT Paramount Solusi Prima', 'paramount@solusiprima.id', '021-5299112', 'SCBD Lot 10, Jakarta Selatan', 1),
(37, 'CUST-037', 'CV Bintang Terang Mandiri', 'bintangterang@gmail.com', '022-7309988', 'Jl. Martadinata No. 50, Bandung', 1),
(38, 'CUST-038', 'PT Global Network Solusi', 'info@globalnetwork.co.id', '021-5701234', 'WTC Sudirman Lt. 10, Jakarta Selatan', 1),
(39, 'CUST-039', 'PT Sentosa Karya Buana', 'sentosakarya@yahoo.com', '031-5349911', 'Jl. Pemuda No. 88, Surabaya', 0),
(40, 'CUST-040', 'CV Mandiri Jaya Perkasa', 'mandirijaya@gmail.com', '024-8445566', 'Jl. Gajah Mada No. 15, Semarang', 1),
(41, 'CUST-041', 'PT Wahana Trans Nusantara', 'wahanatrans@logistics.co.id', '021-4390123', 'Jl. Yos Sudarso No. 27, Jakarta Utara', 1),
(42, 'CUST-042', 'PT Sumber Rizki Abadi', 'sumberrizki@gmail.com', '031-8532112', 'Kawasan Industri Sier Blok H, Surabaya', 1),
(43, 'CUST-043', 'CV Cahaya Baru Teknik', 'cahayabaru@teknik.com', '022-4235566', 'Jl. Braga No. 14, Bandung', 1),
(44, 'CUST-044', 'PT Mitra Perkasa Digital', 'mitraperkasa@digital.id', '021-7912345', 'Jl. Warung Buncit Raya No. 33, Jakarta Selatan', 1),
(45, 'CUST-045', 'PT Artha Buana Mandiri', 'arthabuana@mandiri.co.id', '061-4578901', 'Jl. S. Parman No. 20, Medan', 1),
(46, 'CUST-046', 'CV Makmur Sejahtera Selalu', 'makmursejahtera@yahoo.com', '0274-514433', 'Jl. Malioboro No. 88, Yogyakarta', 1),
(47, 'CUST-047', 'PT Bina Sarana Informatika', 'bsi@informatika.co.id', '021-8091234', 'Jl. Dewi Sartika No. 77, Jakarta Timur', 1),
(48, 'CUST-048', 'PT Graha Cipta Solusi', 'grahacipta@solusi.id', '031-5012345', 'Jl. Kertajaya Indah No. 5, Surabaya', 1),
(49, 'CUST-049', 'CV Karya Utama Mandiri', 'karyautama@gmail.com', '0251-8332112', 'Jl. Pajajaran No. 150, Bogor', 0),
(50, 'CUST-050', 'PT Dinamika Solusi Data', 'dinamika@solusidata.co.id', '021-5204433', 'Menara Imperium Lt. 8, Jakarta Selatan', 1),
(51, 'CUST-051', 'PT Fastabiqul خير Logistik', 'fastabiqul@logistics.com', '022-7561234', 'Jl. Buah Batu No. 210, Bandung', 1),
(52, 'CUST-052', 'PT Omega Digital Perkasa', 'omegadigital@perkasa.id', '031-8712345', 'Jl. Kenjeran No. 90, Surabaya', 1),
(53, 'CUST-053', 'CV Rajawali Sakti Teknik', 'rajawalisakti@gmail.com', '024-8312345', 'Jl. Pemuda No. 11, Semarang', 1),
(54, 'CUST-054', 'PT Sigma Kreasi Mandiri', 'sigmakreasi@mandiri.co.id', '021-7801234', 'Jl. TB Simatupang No. 88, Jakarta Selatan', 1),
(55, 'CUST-055', 'PT Berlian Maju Bersama', 'berlianmaju@gmail.com', '061-4523456', 'Jl. Gatot Subroto No. 112, Medan', 1),
(56, 'CUST-056', 'CV Delta Buana Perkasa', 'deltabuana@perkasa.com', '0274-561234', 'Jl. Gejayan No. 25, Yogyakarta', 1),
(57, 'CUST-057', 'PT Indotama Global Media', 'indotamaglobal@media.id', '031-5456789', 'Jl. Basuki Rahmat No. 100, Surabaya', 1),
(58, 'CUST-058', 'PT Nusantara Tekno Solusi', 'nusandratekno@solusi.co.id', '021-5723456', 'Plaza Sentral Lt. 12, Jakarta Selatan', 1),
(59, 'CUST-059', 'CV Surya Gemilang Abadi', 'suryagemilang@yahoo.com', '022-6012345', 'Jl. Pasirkaliki No. 80, Bandung', 1),
(60, 'CUST-060', 'PT Multi Solusi Data', 'multisolusi@data.id', '021-5256789', 'Granadi Building Lt. 4, Jakarta Selatan', 1),
(61, 'CUST-061', 'PT Mega Surya Perkasa', 'megasurya@perkasa.co.id', '031-8491234', 'Jl. Rungkut Madya No. 5, Surabaya', 1),
(62, 'CUST-062', 'CV Berkah Ilahi Mandiri', 'berkahilahi@gmail.com', '024-8415678', 'Jl. Pandanaran No. 100, Semarang', 1),
(63, 'CUST-063', 'PT Primadona Cipta Solusi', 'primadona@ciptasolusi.id', '021-4289123', 'Jl. Cempaka Putih Tengah No. 4, Jakarta Pusat', 1),
(64, 'CUST-064', 'PT Kencana Indah Logistik', 'kencanaindah@logistics.co.id', '061-4534567', 'Jl. Iskandar Muda No. 45, Medan', 1),
(65, 'CUST-065', 'CV Mitra Mandiri Sejahtera', 'mitramandiri@sejahtera.com', '022-7312345', 'Jl. Gatot Subroto No. 300, Bandung', 1),
(66, 'CUST-066', 'PT Data Solusi Utama', 'datasolusi@utama.id', '021-5791234', 'Menara Jamsostek Lt. 18, Jakarta Selatan', 1),
(67, 'CUST-067', 'PT Cyber Network Nusantara', 'cybernetwork@nusantara.co.id', '031-5034567', 'Jl. Raya Gubeng No. 66, Surabaya', 1),
(68, 'CUST-068', 'CV Anugerah Teknik Mandiri', 'anugerahteknik@gmail.com', '0251-8356789', 'Jl. Pajajaran No. 88, Bogor', 1),
(69, 'CUST-069', 'PT Graha Sarana Informatika', 'grahasarana@info.co.id', '021-7812345', 'Jl. Ampera Raya No. 20, Jakarta Selatan', 1),
(70, 'CUST-070', 'PT Buana Media Solusi', 'buanamedia@solusi.id', '021-5291234', 'MidPlaza Lt. 9, Jakarta Pusat', 1);

-- --------------------------------------------------------

--
-- Table structure for table `departments`
--

CREATE TABLE `departments` (
  `Id` int(11) NOT NULL,
  `Code` varchar(20) NOT NULL,
  `Name` varchar(100) NOT NULL,
  `Description` varchar(500) DEFAULT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `departments`
--

INSERT INTO `departments` (`Id`, `Code`, `Name`, `Description`, `CreatedAt`, `UpdatedAt`) VALUES
(1, 'DPT-001', 'IT Department', 'Information Technology and Support', '2026-10-30 15:59:41.603355', NULL),
(2, 'DPT-002', 'Operational Department', 'Core Business Operations', '2026-11-06 10:16:42.180254', NULL),
(5, 'DPT-003', 'Finance Department', 'Finance and Accounting Division', '2026-11-06 11:00:00.000000', NULL),
(6, 'DPT-004', 'HR Department', 'Human Resources Division', '2026-11-06 11:00:00.000000', NULL),
(7, 'DPT-005', 'Marketing Department', 'Brand Strategy and Public Relations', '2026-11-06 11:00:00.000000', NULL),
(8, 'DPT-006', 'Sales Department', 'Revenue Growth and Client Acquisition', '2026-11-06 11:00:00.000000', NULL),
(9, 'DPT-007', 'Logistics Department', 'Supply Chain and Warehouse Management', '2026-11-06 11:00:00.000000', NULL),
(10, 'DPT-008', 'Legal Department', 'Compliance and Corporate Law', '2026-11-06 11:00:00.000000', NULL),
(11, 'DPT-009', 'Research and Development', 'Product Innovation and Testing', '2026-11-06 11:00:00.000000', NULL),
(12, 'DPT-010', 'Quality Assurance', 'Product Quality Control and Standards', '2026-11-06 11:00:00.000000', NULL),
(13, 'DPT-011', 'Customer Support', 'Client Assistance and Issue Resolution', '2026-11-06 11:00:00.000000', NULL),
(14, 'DPT-012', 'Procurement Department', 'Vendor Management and Purchasing', '2026-11-06 11:00:00.000000', NULL),
(15, 'DPT-013', 'Public Relations', 'Media Relations and Corporate Image', '2026-11-06 11:00:00.000000', NULL),
(16, 'DPT-014', 'Maintenance Department', 'Facility and Equipment Maintenance', '2026-11-06 11:00:00.000000', NULL),
(17, 'DPT-015', 'Security Department', 'Asset Protection and Office Security', '2026-11-06 11:00:00.000000', NULL),
(18, 'DPT-016', 'Training and Development', 'Employee Skill Enhancement Programs', '2026-11-06 11:00:00.000000', NULL),
(19, 'DPT-017', 'Internal Audit', 'Financial and Operational Review', '2026-11-06 11:00:00.000000', NULL),
(20, 'DPT-018', 'Event Management', 'Corporate Events and Exhibitions', '2026-11-06 11:00:00.000000', NULL),
(21, 'DPT-019', 'General Affairs', 'Office Administration and General Services', '2026-11-06 11:00:00.000000', NULL),
(22, 'DPT-020', 'Strategic Planning', 'Business Expansion and Long-term Goals', '2026-11-06 11:00:00.000000', '2026-11-08 14:01:42.609706');

-- --------------------------------------------------------

--
-- Table structure for table `gudang`
--

CREATE TABLE `gudang` (
  `Id` int(11) NOT NULL,
  `KodeGudang` varchar(50) NOT NULL,
  `NamaGudang` varchar(100) NOT NULL,
  `TipeGudang` varchar(20) NOT NULL,
  `PicGudang` varchar(100) NOT NULL,
  `AlamatGudang` varchar(500) DEFAULT NULL,
  `IsActive` tinyint(1) NOT NULL,
  `IsAudit` tinyint(1) NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `gudang`
--

INSERT INTO `gudang` (`Id`, `KodeGudang`, `NamaGudang`, `TipeGudang`, `PicGudang`, `AlamatGudang`, `IsActive`, `IsAudit`, `CreatedAt`, `UpdatedAt`) VALUES
(1, 'GDG-JKT-1', 'Gudang Utama Jakarta', 'fisik', 'Budi Santoso', 'Jl. Industri No. 12, Jakarta Utara', 1, 0, '2026-10-30 15:52:54.416675', '2026-11-08 13:46:28.473474'),
(2, 'GDG-SBY-01', 'Gudang Cabang Surabaya', 'fisik', 'Santoso', 'Kawasan Industri Rungkut Block B-3, Surabaya', 1, 0, '2026-11-01 09:49:25.062669', NULL),
(3, 'GDG-VIR-1', 'Gudang Transit Virtual', 'virtual', 'Sistem Otomatis', 'Cloud Database System', 1, 0, '2026-11-05 09:19:16.896040', NULL),
(4, 'GDG-SAM-1', 'Gudang Cabang Samarinda', 'fisik', 'Sumarno', 'Jln. Kapten Sudirman No 5 Kota Samarinda', 1, 0, '2026-11-16 11:49:06.785540', '2026-11-16 11:49:18.162055'),
(5, 'GDG-SMG-01', 'Gudang Cabang Semarang', 'fisik', 'Hadi Sucipto', 'Kawasan Industri Candi Blok D-5, Semarang', 1, 0, '2026-11-08 10:30:00.000000', NULL),
(6, 'GDG-MLG-01', 'Gudang Cabang Malang', 'fisik', 'Siti Aminah', 'Jl. Sulfat Raya No. 12, Malang', 1, 0, '2026-11-08 11:00:00.000000', NULL),
(7, 'GDG-MDN-01', 'Gudang Cabang Medan', 'fisik', 'Roni Kartono', 'Kawasan Industri Medan (KIM) Tahap II, Medan', 1, 0, '2026-11-08 11:30:00.000000', NULL),
(8, 'GDG-PLB-01', 'Gudang Cabang Palembang', 'fisik', 'Ahmad Zulkarnain', 'Jl. Kolonel H. Burlian No. 88, Palembang', 1, 0, '2026-11-08 12:00:00.000000', NULL),
(9, 'GDG-PKU-01', 'Gudang Cabang Pekanbaru', 'fisik', 'Eka Putra', 'Jl. Soebrantas Panam No. 15, Pekanbaru', 1, 0, '2026-11-08 12:30:00.000000', NULL),
(10, 'GDG-BTM-01', 'Gudang Cabang Batam', 'fisik', 'Dedi Mulyadi', 'Kawasan Batamindo Industrial Park Blok A, Batam', 1, 0, '2026-11-08 13:00:00.000000', NULL),
(11, 'GDG-DPS-01', 'Gudang Cabang Denpasar', 'fisik', 'Wayan Sudana', 'Jl. Sunset Road No. 77, Kuta, Bali', 1, 0, '2026-11-08 13:30:00.000000', NULL),
(12, 'GDG-MKS-01', 'Gudang Cabang Makassar', 'fisik', 'Andi Mallarangeng', 'Kawasan Industri Makassar (KIMA) Raya, Makassar', 1, 0, '2026-11-08 14:00:00.000000', NULL),
(13, 'GDG-BDJ-01', 'Gudang Cabang Banjarmasin', 'fisik', 'Fajar Ramadhan', 'Jl. A. Yani Km. 6, Banjarmasin', 1, 0, '2026-11-08 14:30:00.000000', NULL),
(14, 'GDG-SMD-01', 'Gudang Cabang Samarinda', 'fisik', 'Rahmat Hidayat', 'Jl. Pangeran Antasari No. 45, Samarinda', 1, 0, '2026-11-08 15:00:00.000000', NULL),
(15, 'GDG-PNK-01', 'Gudang Cabang Pontianak', 'fisik', 'Heri Susanto', 'Jl. Ayani No. 102, Pontianak', 1, 0, '2026-11-08 15:30:00.000000', NULL),
(16, 'GDG-MDO-01', 'Gudang Cabang Manado', 'fisik', 'Stenly Roring', 'Jl. Sam Ratulangi No. 88, Manado', 1, 0, '2026-11-08 16:00:00.000000', NULL),
(17, 'GDG-YOG-01', 'Gudang Cabang Yogyakarta', 'fisik', 'Bambang Pamungkas', 'Jl. Ring Road Utara No. 10, Sleman, Yogyakarta', 1, 0, '2026-11-08 16:30:00.000000', NULL),
(18, 'GDG-SOL-01', 'Gudang Cabang Solo', 'fisik', 'Joko Supriyanto', 'Jl. Adi Sucipto No. 50, Solo', 1, 0, '2026-11-08 17:00:00.000000', NULL),
(19, 'GDG-LMP-01', 'Gudang Cabang Lampung', 'fisik', 'Arief Budiman', 'Jl. ZA Pagar Alam No. 25, Bandar Lampung', 1, 0, '2026-11-08 17:30:00.000000', NULL),
(20, 'GDG-PDG-01', 'Gudang Cabang Padang', 'fisik', 'Rizal Fahmi', 'Jl. Khatib Sulaiman No. 19, Padang', 1, 0, '2026-11-08 18:00:00.000000', NULL),
(21, 'GDG-JBI-01', 'Gudang Cabang Jambi', 'fisik', 'Hendra Wijaya', 'Jl. Pangeran Hidayat No. 8, Jambi', 1, 0, '2026-11-08 18:30:00.000000', NULL),
(22, 'GDG-BKS-01', 'Gudang Bekasi Timur', 'fisik', 'Slamet Riyadi', 'Jl. Joyo Martono No. 12, Bekasi Timur', 1, 0, '2026-11-08 19:00:00.000000', NULL),
(23, 'GDG-TGR-01', 'Gudang Tangerang Modern', 'fisik', 'Cahyono', 'Kawasan Pergudangan 3 In 1, Tangerang', 1, 0, '2026-11-08 19:30:00.000000', NULL),
(24, 'GDG-BGR-01', 'Gudang Bogor Sentul', 'fisik', 'Agus Setiawan', 'Kawasan Industri Sentul Blok C, Bogor', 1, 0, '2026-11-08 20:00:00.000000', NULL),
(25, 'GDG-CRB-01', 'Gudang Cirebon Argasunya', 'fisik', 'Diki Wahyudi', 'Jl. By Pass Sudirman No. 99, Cirebon', 1, 0, '2026-11-08 20:30:00.000000', NULL),
(26, 'GDG-PWT-01', 'Gudang Purwokerto', 'fisik', 'Imam Syafi\'i', 'Jl. Jenderal Soedirman No. 200, Purwokerto', 1, 0, '2026-11-08 21:00:00.000000', NULL),
(27, 'GDG-KDR-01', 'Gudang Kediri', 'fisik', 'Yudi Santoso', 'Jl. Dhoho No. 45, Kediri', 1, 0, '2026-11-08 21:30:00.000000', NULL),
(28, 'GDG-JMR-01', 'Gudang Jember', 'fisik', 'Rian Hidayat', 'Jl. Gajah Mada No. 88, Jember', 1, 0, '2026-11-08 22:00:00.000000', NULL),
(29, 'GDG-BLI-02', 'Gudang Singaraja Bali', 'fisik', 'Kadek Suartana', 'Jl. Ahmad Yani No. 14, Singaraja', 1, 0, '2026-11-08 22:30:00.000000', NULL),
(30, 'GDG-LOMBOK', 'Gudang Mataram Lombok', 'fisik', 'Lalu Darmawan', 'Jl. Catur Warga No. 21, Mataram', 1, 0, '2026-11-08 23:00:00.000000', NULL),
(31, 'GDG-KUPANG', 'Gudang Kupang', 'fisik', 'Yohanes Nalle', 'Jl. El Tari No. 50, Kupang', 1, 0, '2026-11-08 23:30:00.000000', NULL),
(32, 'GDG-AMBON', 'Gudang Ambon', 'fisik', 'Marthen Titaley', 'Jl. Dr. Malaiholo No. 12, Ambon', 1, 0, '2026-11-09 00:00:00.000000', NULL),
(33, 'GDG-SORONG', 'Gudang Sorong', 'fisik', 'Frits Mayor', 'Jl. Jend. Sudirman Km. 8, Sorong', 1, 0, '2026-11-09 00:30:00.000000', NULL),
(34, 'GDG-JAYAPURA', 'Gudang Jayapura', 'fisik', 'Corneles Suebu', 'Jl. Abepura No. 101, Jayapura', 1, 0, '2026-11-09 01:00:00.000000', NULL),
(35, 'GDG-TIMIKA', 'Gudang Timika', 'fisik', 'Markus Wanggai', 'Jl. Cenderawasih No. 33, Timika', 1, 0, '2026-11-09 01:30:00.000000', NULL),
(36, 'GDG-VIR-2', 'Gudang Cadangan Cloud AWS', 'virtual', 'Cloud System 2', 'Amazon Web Services (AWS) Region Jakarta', 1, 0, '2026-11-09 02:00:00.000000', NULL),
(37, 'GDG-VIR-3', 'Gudang Cadangan Cloud GCP', 'virtual', 'Cloud System 3', 'Google Cloud Platform (GCP) Region Singapore', 1, 0, '2026-11-09 02:30:00.000000', NULL),
(38, 'GDG-ARSIP-01', 'Gudang Arsip Pusat Jakarta', 'fisik', 'Suharto', 'Jl. Fatmawati Raya No. 45, Jakarta Selatan', 1, 0, '2026-11-09 03:00:00.000000', NULL),
(39, 'GDG-ARSIP-02', 'Gudang Arsip Cabang Surabaya', 'fisik', 'Budi Harsono', 'Jl. Gubeng Pojok No. 11, Surabaya', 1, 0, '2026-11-09 03:30:00.000000', NULL),
(40, 'GDG-RETUR-01', 'Gudang Retur Barang Jakarta', 'fisik', 'Dewi Lestari', 'Kawasan Industri Pulogadung Blok H, Jakarta Timur', 1, 0, '2026-11-09 04:00:00.000000', NULL),
(41, 'GDG-RETUR-02', 'Gudang Retur Barang Surabaya', 'fisik', 'Siti Rahma', 'Kawasan Industri Rungkut Blok A, Surabaya', 1, 0, '2026-11-09 04:30:00.000000', NULL),
(42, 'GDG-SPARE-01', 'Gudang Suku Cadang Mesin Utama', 'fisik', 'Tono Sutono', 'Jl. Industri Utama No. 5, Cikarang', 1, 0, '2026-11-09 05:00:00.000000', NULL),
(43, 'GDG-SPARE-02', 'Gudang Sparepart IT & Elektronik', 'fisik', 'Rian Kurniawan', 'Gedung Cyber Lt. 2, Jakarta Selatan', 1, 0, '2026-11-09 05:30:00.000000', NULL),
(44, 'GDG-TRANS-02', 'Gudang Transit Antar Pulau', 'virtual', 'Logistics Bot', 'Internal Transit Server', 1, 0, '2026-11-09 06:00:00.000000', NULL),
(45, 'GDG-TRANS-03', 'Gudang Transit Ekspor', 'virtual', 'Export System', 'Global Gateway Server', 1, 0, '2026-11-09 06:30:00.000000', NULL),
(46, 'GDG-SEASONAL-1', 'Gudang Musiman Promo Akhir Tahun', 'fisik', 'Doni Setiawan', 'Jl. Danau Sunter Utara Blok M, Jakarta Utara', 1, 0, '2026-11-09 07:00:00.000000', NULL),
(47, 'GDG-SEASONAL-2', 'Gudang Musiman Lebaran', 'fisik', 'Feri Irawan', 'Jl. Kalimalang Raya No. 88, Bekasi', 1, 0, '2026-11-09 07:30:00.000000', NULL),
(48, 'GDG-RAW-01', 'Gudang Bahan Baku Utama', 'fisik', 'Haryanto', 'Kawasan Industri Karawang (KIIC) Lot B, Karawang', 1, 0, '2026-11-09 08:00:00.000000', NULL),
(49, 'GDG-RAW-02', 'Gudang Bahan Baku Pendukung', 'fisik', 'Sugeng', 'Kawasan Industri Cikande Blok F, Serang', 1, 0, '2026-11-09 08:30:00.000000', NULL),
(50, 'GDG-FIN-01', 'Gudang Barang Jadi (Finished Goods) Jkt', 'fisik', 'Arif Rahman', 'Jl. Daan Mogot Km. 19, Cengkareng, Jakarta Barat', 1, 0, '2026-11-09 09:00:00.000000', NULL),
(51, 'GDG-FIN-02', 'Gudang Barang Jadi (Finished Goods) Sby', 'fisik', 'Bambang Irawan', 'Jl. Margomulyo Indah No. 10, Surabaya', 1, 0, '2026-11-09 09:30:00.000000', NULL),
(52, 'GDG-HQ-01', 'Gudang Logistik Kantor Pusat', 'fisik', 'Admin Utama', 'Komp. Perkantoran Indah Blok A-1, Jakarta Selatan', 1, 0, '2026-11-09 10:00:00.000000', NULL),
(53, 'GDG-QA-01', 'Gudang Karantina & Quality Control', 'fisik', 'Drh. Mulyadi', 'Jl. Laboratorium Teknik No. 7, Bandung', 1, 0, '2026-11-09 10:30:00.000000', NULL);

-- --------------------------------------------------------

--
-- Table structure for table `karyawans`
--

CREATE TABLE `karyawans` (
  `Id` int(11) NOT NULL,
  `Nik` longtext NOT NULL,
  `NamaLengkap` longtext NOT NULL,
  `Email` longtext NOT NULL,
  `NoTelepon` longtext NOT NULL,
  `DepartmentId` int(11) NOT NULL,
  `Status` longtext NOT NULL,
  `Jabatan` longtext DEFAULT NULL,
  `UrlFoto` longtext DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `karyawans`
--

INSERT INTO `karyawans` (`Id`, `Nik`, `NamaLengkap`, `Email`, `NoTelepon`, `DepartmentId`, `Status`, `Jabatan`, `UrlFoto`) VALUES
(1, 'EMP-001', 'Budi Santoso', 'budi.santoso@email.com', '081234567801', 1, 'Aktif', 'Software Engineer', 'images/karyawan/0bd67b70-749c-4171-ad5a-2f15f5cb3f50.png'),
(2, 'EMP-002', 'Siti Rahmawati', 'siti.rahmawati@email.com', '081234567802', 2, 'Aktif', 'Operations Staff', 'images/karyawan/4077d06d-4b49-4a6b-a3b8-1fc3837234a5.png'),
(3, 'EMP-003', 'Ahmad Fauzi', 'ahmad.fauzi@email.com', '081234567803', 5, 'Aktif', 'Accountant', 'images/karyawan/0bd67b70-749c-4171-ad5a-2f15f5cb3f50.png'),
(4, 'EMP-004', 'Dewi Lestari', 'dewi.lestari@email.com', '081234567804', 6, 'Aktif', 'HR Specialist', 'images/karyawan/4077d06d-4b49-4a6b-a3b8-1fc3837234a5.png'),
(5, 'EMP-005', 'Eko Prasetyo', 'eko.prasetyo@email.com', '081234567805', 7, 'Aktif', 'Marketing Executive', 'images/karyawan/0bd67b70-749c-4171-ad5a-2f15f5cb3f50.png'),
(6, 'EMP-006', 'Rina Amelia', 'rina.amelia@email.com', '081234567806', 8, 'Aktif', 'Sales Representative', 'images/karyawan/4077d06d-4b49-4a6b-a3b8-1fc3837234a5.png'),
(7, 'EMP-007', 'Joko Widodo', 'joko.widodo@email.com', '081234567807', 9, 'Aktif', 'Logistics Coordinator', 'images/karyawan/0bd67b70-749c-4171-ad5a-2f15f5cb3f50.png'),
(8, 'EMP-008', 'Sri Wahyuni', 'sri.wahyuni@email.com', '081234567808', 10, 'Aktif', 'Legal Counsel', 'images/karyawan/4077d06d-4b49-4a6b-a3b8-1fc3837234a5.png'),
(9, 'EMP-009', 'Rizki Pratama', 'rizki.pratama@email.com', '081234567809', 11, 'Aktif', 'R&D Researcher', 'images/karyawan/0bd67b70-749c-4171-ad5a-2f15f5cb3f50.png'),
(10, 'EMP-010', 'Nurul Hidayah', 'nurul.hidayah@email.com', '081234567810', 12, 'Aktif', 'QA Analyst', 'images/karyawan/4077d06d-4b49-4a6b-a3b8-1fc3837234a5.png'),
(11, 'EMP-011', 'Hendra Wijaya', 'hendra.wijaya@email.com', '081234567811', 13, 'Aktif', 'Support Agent', 'images/karyawan/0bd67b70-749c-4171-ad5a-2f15f5cb3f50.png'),
(12, 'EMP-012', 'Dian Puspita', 'dian.puspita@email.com', '081234567812', 14, 'Aktif', 'Procurement Officer', 'images/karyawan/4077d06d-4b49-4a6b-a3b8-1fc3837234a5.png'),
(13, 'EMP-013', 'Dedi Kurniawan', 'dedi.kurniawan@email.com', '081234567813', 15, 'Aktif', 'PR Specialist', 'images/karyawan/0bd67b70-749c-4171-ad5a-2f15f5cb3f50.png'),
(14, 'EMP-014', 'Wulan Dari', 'wulan.dari@email.com', '081234567814', 16, 'Aktif', 'Maintenance Staff', 'images/karyawan/4077d06d-4b49-4a6b-a3b8-1fc3837234a5.png'),
(15, 'EMP-015', 'Fajar Nugroho', 'fajar.nugroho@email.com', '081234567815', 17, 'Aktif', 'Security Guard', 'images/karyawan/0bd67b70-749c-4171-ad5a-2f15f5cb3f50.png'),
(16, 'EMP-016', 'Anita Rahayu', 'anita.rahayu@email.com', '081234567816', 18, 'Aktif', 'Trainer', 'images/karyawan/4077d06d-4b49-4a6b-a3b8-1fc3837234a5.png'),
(17, 'EMP-017', 'Bayu Saputra', 'bayu.saputra@email.com', '081234567817', 19, 'Aktif', 'Internal Auditor', 'images/karyawan/0bd67b70-749c-4171-ad5a-2f15f5cb3f50.png'),
(18, 'EMP-018', 'Indah Permata', 'indah.permata@email.com', '081234567818', 20, 'Aktif', 'Event Organizer', 'images/karyawan/4077d06d-4b49-4a6b-a3b8-1fc3837234a5.png'),
(19, 'EMP-019', 'Galih Pratama', 'galih.pratama@email.com', '081234567819', 21, 'Aktif', 'Admin Staff', 'images/karyawan/0bd67b70-749c-4171-ad5a-2f15f5cb3f50.png'),
(20, 'EMP-020', 'Mega Lestari', 'mega.lestari@email.com', '081234567820', 22, 'Aktif', 'Strategic Planner', 'images/karyawan/4077d06d-4b49-4a6b-a3b8-1fc3837234a5.png'),
(21, 'EMP-021', 'Ardiansyah', 'ardiansyah@email.com', '081234567821', 1, 'Aktif', 'System Administrator', 'images/karyawan/0bd67b70-749c-4171-ad5a-2f15f5cb3f50.png'),
(22, 'EMP-022', 'Ratna Sari', 'ratna.sari@email.com', '081234567822', 2, 'Aktif', 'Operations Supervisor', 'images/karyawan/4077d06d-4b49-4a6b-a3b8-1fc3837234a5.png'),
(23, 'EMP-023', 'Doni Setiawan', 'doni.setiawan@email.com', '081234567823', 5, 'Aktif', 'Financial Analyst', 'images/karyawan/0bd67b70-749c-4171-ad5a-2f15f5cb3f50.png'),
(24, 'EMP-024', 'Maya Anggraini', 'maya.anggraini@email.com', '081234567824', 6, 'Aktif', 'Recruiter', 'images/karyawan/4077d06d-4b49-4a6b-a3b8-1fc3837234a5.png'),
(25, 'EMP-025', 'Rahmat Hidayat', 'rahmat.hidayat@email.com', '081234567825', 7, 'Aktif', 'Brand Manager', 'images/karyawan/0bd67b70-749c-4171-ad5a-2f15f5cb3f50.png'),
(26, 'EMP-026', 'Fitri Handayani', 'fitri.handayani@email.com', '081234567826', 8, 'Aktif', 'Sales Executive', 'images/karyawan/4077d06d-4b49-4a6b-a3b8-1fc3837234a5.png'),
(27, 'EMP-027', 'Yogi Pratama', 'yogi.pratama@email.com', '081234567827', 9, 'Aktif', 'Warehouse Staff', 'images/karyawan/0bd67b70-749c-4171-ad5a-2f15f5cb3f50.png'),
(28, 'EMP-028', 'Siska Amelia', 'siska.amelia@email.com', '081234567828', 10, 'Aktif', 'Compliance Officer', 'images/karyawan/4077d06d-4b49-4a6b-a3b8-1fc3837234a5.png'),
(29, 'EMP-029', 'Agung Prasetyo', 'agung.prasetyo@email.com', '081234567829', 11, 'Aktif', 'Product Tester', 'images/karyawan/0bd67b70-749c-4171-ad5a-2f15f5cb3f50.png'),
(30, 'EMP-030', 'Yuni Astuti', 'yuni.astuti@email.com', '081234567830', 12, 'Aktif', 'Quality Control', 'images/karyawan/4077d06d-4b49-4a6b-a3b8-1fc3837234a5.png'),
(31, 'EMP-031', 'Reza Pahlevi', 'reza.pahlevi@email.com', '081234567831', 13, 'Aktif', 'Helpdesk Support', 'images/karyawan/0bd67b70-749c-4171-ad5a-2f15f5cb3f50.png'),
(32, 'EMP-032', 'Dwi Utami', 'dwi.utami@email.com', '081234567832', 14, 'Aktif', 'Vendor Relations', 'images/karyawan/4077d06d-4b49-4a6b-a3b8-1fc3837234a5.png'),
(33, 'EMP-033', 'Irfan Hakim', 'irfan.hakim@email.com', '081234567833', 15, 'Aktif', 'Media Relations', 'images/karyawan/0bd67b70-749c-4171-ad5a-2f15f5cb3f50.png'),
(34, 'EMP-034', 'Kartika Putri', 'kartika.putri@email.com', '081234567834', 16, 'Aktif', 'Facility Assistant', 'images/karyawan/4077d06d-4b49-4a6b-a3b8-1fc3837234a5.png'),
(35, 'EMP-035', 'Wahyu Hidayat', 'wahyu.hidayat@email.com', '081234567835', 17, 'Aktif', 'Security Supervisor', 'images/karyawan/0bd67b70-749c-4171-ad5a-2f15f5cb3f50.png'),
(36, 'EMP-036', 'Nining Sunarti', 'nining.sunarti@email.com', '081234567836', 18, 'Aktif', 'Training Coordinator', 'images/karyawan/0bd67b70-749c-4171-ad5a-2f15f5cb3f50.png'),
(37, 'EMP-037', 'Gilang Ramadhan', 'gilang.ramadhan@email.com', '081234567837', 19, 'Aktif', 'Junior Auditor', 'images/karyawan/0bd67b70-749c-4171-ad5a-2f15f5cb3f50.png'),
(38, 'EMP-038', 'Tiara Anugrah', 'tiara.anugrah@email.com', '081234567838', 20, 'Aktif', 'Event Coordinator', 'images/karyawan/4077d06d-4b49-4a6b-a3b8-1fc3837234a5.png'),
(39, 'EMP-039', 'Bagas Mahendra', 'bagas.mahendra@email.com', '081234567839', 21, 'Aktif', 'Office Assistant', 'images/karyawan/0bd67b70-749c-4171-ad5a-2f15f5cb3f50.png'),
(40, 'EMP-040', 'Lesti Kejora', 'lesti.kejora@email.com', '081234567840', 22, 'Aktif', 'Business Analyst', 'images/karyawan/4077d06d-4b49-4a6b-a3b8-1fc3837234a5.png'),
(41, 'EMP-041', 'Dimas Anggara', 'dimas.anggara@email.com', '081234567841', 1, 'Aktif', 'Network Engineer', 'images/karyawan/0bd67b70-749c-4171-ad5a-2f15f5cb3f50.png'),
(42, 'EMP-042', 'Nadia Salsabila', 'nadia.salsabila@email.com', '081234567842', 2, 'Aktif', 'Operations Admin', 'images/karyawan/4077d06d-4b49-4a6b-a3b8-1fc3837234a5.png'),
(43, 'EMP-043', 'Fikri Haikal', 'fikri.haikal@email.com', '081234567843', 5, 'Aktif', 'Tax Specialist', 'images/karyawan/0bd67b70-749c-4171-ad5a-2f15f5cb3f50.png'),
(44, 'EMP-044', 'Zahra Aulia', 'zahra.aulia@email.com', '081234567844', 6, 'Aktif', 'HR Admin', 'images/karyawan/4077d06d-4b49-4a6b-a3b8-1fc3837234a5.png'),
(45, 'EMP-045', 'Rendy Septian', 'rendy.septian@email.com', '081234567845', 7, 'Aktif', 'Copywriter', 'images/karyawan/0bd67b70-749c-4171-ad5a-2f15f5cb3f50.png'),
(46, 'EMP-046', 'Safira Nabila', 'safira.nabila@email.com', '081234567846', 8, 'Aktif', 'Sales Admin', 'images/karyawan/4077d06d-4b49-4a6b-a3b8-1fc3837234a5.png'),
(47, 'EMP-047', 'Iqbal Ramadhan', 'iqbal.ramadhan@email.com', '081234567847', 9, 'Aktif', 'Inventory Keeper', 'images/karyawan/0bd67b70-749c-4171-ad5a-2f15f5cb3f50.png'),
(48, 'EMP-048', 'Tasya Kamila', 'tasya.kamila@email.com', '081234567848', 10, 'Aktif', 'Legal Assistant', 'images/karyawan/4077d06d-4b49-4a6b-a3b8-1fc3837234a5.png'),
(49, 'EMP-049', 'Fauzan Al Ghifari', 'fauzan.alghifari@email.com', '081234567849', 11, 'Aktif', 'Lab Assistant', 'images/karyawan/0bd67b70-749c-4171-ad5a-2f15f5cb3f50.png'),
(50, 'EMP-050', 'Roro Mendut', 'roro.mendut@email.com', '081234567850', 12, 'Aktif', 'Standard Auditor', 'images/karyawan/4077d06d-4b49-4a6b-a3b8-1fc3837234a5.png');

-- --------------------------------------------------------

--
-- Table structure for table `kategoris`
--

CREATE TABLE `kategoris` (
  `Id` int(11) NOT NULL,
  `NamaKategori` varchar(100) NOT NULL,
  `Deskripsi` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `kategoris`
--

INSERT INTO `kategoris` (`Id`, `NamaKategori`, `Deskripsi`) VALUES
(1, 'Consumables', 'Consumables'),
(2, 'IT Accessories', 'Untuk perangkat IT Accessories'),
(3, 'IT Equipment', 'Untuk peralatan IT'),
(4, 'Office Supplies', 'Perlengkapan dan alat tulis kantor'),
(5, 'Networking Devices', 'Perangkat jaringan, switch, dan router'),
(6, 'Server & Storage', 'Perangkat server dan media penyimpanan data'),
(7, 'Furniture Kantor', 'Meja, kursi, dan lemari arsip ruangan'),
(8, 'Elektronik Ruangan', 'AC, kipas angin, dan pendingin ruangan'),
(9, 'Alat Kebersihan', 'Peralatan dan bahan pembersih kantor'),
(10, 'Perlengkapan Keamanan', 'CCTV, brankas, dan alat akses kontrol'),
(11, 'Alat Ukur & Testing', 'Multimeter dan alat kalibrasi teknis'),
(12, 'Perangkat Audio Visual', 'Proyektor, layar, dan speaker meeting'),
(13, 'Kendaraan Operasional', 'Kendaraan dinas roda dua dan roda empat'),
(14, 'Alat Komunikasi', 'Telepon IP dan perangkat komunikasi'),
(15, 'Perlengkapan P3K', 'Kotak obat dan alat kesehatan darurat'),
(16, 'Perangkat Hardware Pendukung', 'Printer, scanner, dan UPS'),
(17, 'Software & Lisensi', 'Perangkat lunak dan sistem operasional'),
(18, 'Merchandise & Souvenir', 'Atribut dan cinderamata perusahaan'),
(19, 'Alat Laboratorium', 'Peralatan uji coba dan riset khusus'),
(20, 'Material Logistik', 'Bahan baku dan kemasan gudang');

-- --------------------------------------------------------

--
-- Table structure for table `master_uom`
--

CREATE TABLE `master_uom` (
  `Id` int(11) NOT NULL,
  `Code` varchar(10) NOT NULL,
  `Name` varchar(50) NOT NULL,
  `Description` varchar(255) DEFAULT NULL,
  `IsActive` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `master_uom`
--

INSERT INTO `master_uom` (`Id`, `Code`, `Name`, `Description`, `IsActive`) VALUES
(1, 'PCS', 'Pieces', 'Satuan hitung untuk barang satuan atau bijian', 1),
(2, 'UNIT', 'Unit', 'Satuan untuk barang utuh atau peralatan', 1),
(3, 'SET', 'Set', 'Satuan untuk barang yang terdiri dari beberapa bagian', 1),
(4, 'BOX', 'Box', 'Satuan kemasan kotak kecil atau sedang', 1),
(5, 'CTN', 'Carton', 'Satuan kemasan kardus besar', 1),
(6, 'PKT', 'Packet', 'Satuan kemasan paket kecil', 1),
(7, 'PAC', 'Pack', 'Satuan kemasan bungkus', 1),
(8, 'ROL', 'Roll', 'Satuan untuk barang gulungan seperti kabel atau plastik', 1),
(9, 'BDL', 'Bundle', 'Satuan ikatan atau bendelan', 1),
(10, 'BAG', 'Bag', 'Satuan kantong atau sak', 1),
(11, 'SACK', 'Sack', 'Satuan karung untuk beras, semen, dll', 1),
(12, 'KG', 'Kilogram', 'Satuan berat kilogram', 1),
(13, 'GR', 'Gram', 'Satuan berat gram', 1),
(14, 'TON', 'Ton', 'Satuan berat ton', 1),
(15, 'ONS', 'Ons', 'Satuan berat ons', 1),
(16, 'M', 'Meter', 'Satuan panjang meter', 1),
(17, 'CM', 'Centimeter', 'Satuan panjang centimeter', 1),
(18, 'LTR', 'Liter', 'Satuan volume cairan liter', 1),
(19, 'ML', 'Milliliter', 'Satuan volume cairan milliliter', 1),
(20, 'LSN', 'Dozen', 'Satuan lusin (12 pcs)', 1),
(21, 'GLN', 'Gallon', 'Satuan galon untuk cairan', 1);

-- --------------------------------------------------------

--
-- Table structure for table `menus`
--

CREATE TABLE `menus` (
  `Id` int(11) NOT NULL,
  `MenuTitle` varchar(100) NOT NULL,
  `Url` varchar(255) NOT NULL,
  `GroupName` varchar(100) NOT NULL,
  `ParentMenu` varchar(100) DEFAULT NULL,
  `SortOrder` int(11) NOT NULL,
  `IconClass` varchar(50) NOT NULL,
  `RequireRoles` longtext DEFAULT NULL,
  `RequirePermissions` longtext DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `menus`
--

INSERT INTO `menus` (`Id`, `MenuTitle`, `Url`, `GroupName`, `ParentMenu`, `SortOrder`, `IconClass`, `RequireRoles`, `RequirePermissions`) VALUES
(8, 'Dashboard', '/Dashboard/Index', 'Main Menu', NULL, 1, 'bi-speedometer2', 'Super Admin,Manager,User,Warehouse Manager,Warehouse Staff,Admin', 'view dashboard'),
(10, 'Panduan Pengguna', '/Guide/Index', 'Main Menu', NULL, 2, 'bi-book', 'Super Admin,Manager,User,Warehouse Manager,Warehouse Staff,Admin', 'view user guide'),
(12, 'Manajemen Gudang', '/Warehouses/Index', 'Inventory Management', NULL, 1, 'bi-building', 'Super Admin,Manager,User,Warehouse Manager,Warehouse Staff,Admin', 'manage warehouses'),
(13, 'Katalog Produk', '/Produk/Index', 'Inventory Management', NULL, 5, 'bi-box-seam', 'Super Admin,Manager,User,Warehouse Manager,Warehouse Staff,Admin', 'manage products'),
(14, 'Supplier', '/Supplier/Index', 'Inventory Management', NULL, 6, 'bi-shop', 'Super Admin,Manager,User,Warehouse Manager,Warehouse Staff,Admin', 'manage suppliers'),
(15, 'Karyawan', '/Karyawan/Index', 'Inventory Management', NULL, 7, 'bi-people', 'Super Admin,Manager,User,Warehouse Manager,Admin', 'manage employees'),
(16, 'Departemen', '/Department/Index', 'Inventory Management', NULL, 8, 'bi-building', 'Super Admin,Manager,User,Warehouse Manager,Admin', 'manage departments'),
(17, 'Customer', '/Customer/Index', 'Inventory Management', NULL, 9, 'bi-people-fill', 'Super Admin,Manager,User,Warehouse Manager,Admin', 'manage customer'),
(18, 'Penugasan Aset', '/PenugasanAset/Index', 'Asset Tracking', NULL, 1, 'bi-person-check', 'Super Admin,Manager,User,Warehouse Manager,Warehouse Staff,Admin', 'manage assignments'),
(19, 'Peminjaman Aset', '/PeminjamanAset/Index', 'Asset Tracking', NULL, 2, 'bi-calendar-week', 'Super Admin,Manager,User,Warehouse Manager,Warehouse Staff,Admin', 'manage loans'),
(20, 'Transaksi Stok In/Out', '/TransaksiStokInOut/Index', 'Stok & Mutasi', NULL, 1, 'bi-calendar-week', 'Super Admin,Manager,User,Warehouse Manager,Warehouse Staff,Admin', 'manage stock transactions'),
(21, 'Transfer Order', '/TransferOrder/Index', 'Stok & Mutasi', NULL, 2, 'bi-truck', 'Super Admin,Manager,User,Warehouse Manager,Warehouse Staff,Admin', 'manage transfer orders'),
(22, 'Stock Opname (Audit)', '/StockOpname/Index', 'Stok & Mutasi', NULL, 3, 'bi-clipboard-check', 'Super Admin,Manager,User,Warehouse Manager,Warehouse Staff,Admin', 'manage stock opname'),
(23, 'Matriks Stok Grid', '/MatriksStok/Index', 'Pelaporan & Analisis', NULL, 1, 'bi-layers', 'Super Admin,Manager,User,Warehouse Manager,Warehouse Staff,Admin', 'view stock matrix'),
(24, 'Kartu Stok (Audit)', '/KartuStok/Index', 'Pelaporan & Analisis', NULL, 2, 'bi-clock', 'Super Admin,Manager,User,Warehouse Manager,Warehouse Staff,Admin', 'view stock cards'),
(25, 'Laporan Barang Masuk/Keluar', '/LaporanBarangMasukKeluar/Index', 'Pelaporan & Analisis', NULL, 3, 'bi-arrow-left-right', 'Super Admin,Manager,User,Warehouse Manager,Warehouse Staff,Admin', 'view stock in/out reports'),
(26, 'Laporan Stock Opname', '/LaporanStockOpname/Index', 'Pelaporan & Analisis', NULL, 4, 'bi-clipboard-check', 'Super Admin,Manager,User,Warehouse Manager,Warehouse Staff,Admin', 'manage stock opname'),
(27, 'Laporan Aset & Peminjaman', '/LaporanAsset/Index', 'Pelaporan & Analisis', NULL, 5, 'bi-tags', 'Super Admin,Manager,User,Warehouse Manager,Warehouse Staff,Admin', 'view asset reports'),
(28, 'System Settings', '/SystemSettings/Index', 'Settings', NULL, 1, 'bi-gear', 'Super Admin', 'manage settings'),
(29, 'User Management', '/UserManagement/Index', 'Settings', NULL, 2, 'bi-people', 'Super Admin', 'manage users'),
(30, 'Roles & Permissions', '/RolesPermissions/Index', 'Settings', NULL, 3, 'bi-shield-check', 'Super Admin', 'manage roles and permissions'),
(31, 'Menu Manager', '/MenuManager/Index', 'Settings', NULL, 4, 'bi-list', 'Super Admin', 'manage settings'),
(34, 'Activity Logs', '/ActivityLogs/Index', 'Other', NULL, 1, 'bi-clock-history', 'Super Admin', 'view logs'),
(35, 'Kategori', '/Kategori/Index', 'Inventory Management', NULL, 4, 'bi-box-seam', 'Super Admin,Manager,User,Warehouse Manager,Admin', 'manage categories'),
(36, 'UOM', '/MasterUom/Index', 'Inventory Management', NULL, 3, 'bi-file-earmark-text', 'Super Admin,Manager,User,Warehouse Manager,Admin', 'manage uom'),
(37, 'Rak', '/Raks/Index', 'Inventory Management', NULL, 2, 'bi-book', 'Super Admin,Manager,User,Warehouse Manager,Admin', 'manage racks'),
(38, 'Reparasi Assets', '/ReparasiAsets/Index', 'Asset Tracking', NULL, 3, 'bi-clipboard-check', 'Super Admin,Manager,User,Warehouse Manager,Warehouse Staff,Admin', 'manage reparasi aset');

-- --------------------------------------------------------

--
-- Table structure for table `peminjamanaset`
--

CREATE TABLE `peminjamanaset` (
  `Id` int(11) NOT NULL,
  `KodePeminjaman` varchar(50) NOT NULL,
  `TanggalPinjam` datetime(6) NOT NULL,
  `TanggalJatuhTempo` datetime(6) NOT NULL,
  `TanggalPengembalian` datetime(6) DEFAULT NULL,
  `TujuanPeminjaman` varchar(500) DEFAULT NULL,
  `JumlahHariPinjam` int(11) DEFAULT NULL,
  `JumlahHariTelatPinjam` int(11) DEFAULT NULL,
  `Status` int(11) DEFAULT NULL,
  `GudangId` int(11) NOT NULL,
  `KaryawanId` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `peminjamanaset`
--

INSERT INTO `peminjamanaset` (`Id`, `KodePeminjaman`, `TanggalPinjam`, `TanggalJatuhTempo`, `TanggalPengembalian`, `TujuanPeminjaman`, `JumlahHariPinjam`, `JumlahHariTelatPinjam`, `Status`, `GudangId`, `KaryawanId`) VALUES
(1, 'LN-202610-0001', '2026-10-30 00:00:00.000000', '2026-10-30 00:00:00.000000', '2026-11-01 00:00:00.000000', NULL, 0, 0, 1, 1, 1),
(2, 'LN-202610-0002', '2026-10-30 00:00:00.000000', '2026-10-30 00:00:00.000000', '2026-10-30 00:00:00.000000', NULL, 0, 0, 2, 1, 1),
(3, 'LN-202610-0003', '2026-10-31 00:00:00.000000', '2026-10-31 00:00:00.000000', NULL, NULL, 0, 0, 1, 1, 2),
(4, 'LN-202611-0001', '2026-11-01 00:00:00.000000', '2026-11-01 00:00:00.000000', NULL, NULL, 0, 0, 1, 1, 2),
(5, 'LN-202611-0002', '2026-11-02 00:00:00.000000', '2026-11-05 00:00:00.000000', '2026-11-05 00:00:00.000000', 'Keperluan presentasi proyek klien di luar kota', 3, 0, 1, 1, 1),
(6, 'LN-202611-0003', '2026-11-03 00:00:00.000000', '2026-11-06 00:00:00.000000', NULL, 'Peminjaman sementara unit laptop untuk perbaikan', 3, 0, 1, 1, 2),
(7, 'LN-202611-0004', '2026-11-04 00:00:00.000000', '2026-11-08 00:00:00.000000', '2026-11-08 00:00:00.000000', 'Audit keuangan cabang', 4, 0, 1, 1, 3),
(8, 'LN-202611-0005', '2026-11-05 00:00:00.000000', '2026-11-10 00:00:00.000000', NULL, 'Pelatihan karyawan baru divisi HR', 5, 0, 1, 1, 4),
(9, 'LN-202611-0006', '2026-11-06 00:00:00.000000', '2026-11-12 00:00:00.000000', NULL, 'Pameran produk dan pameran dagang', 6, 0, 1, 1, 5),
(10, 'LN-202611-0007', '2026-11-07 00:00:00.000000', '2026-11-14 00:00:00.000000', '2026-11-13 00:00:00.000000', 'Kunjungan sales ke klien area timur', 7, 0, 1, 1, 6),
(11, 'LN-202611-0008', '2026-11-08 00:00:00.000000', '2026-11-15 00:00:00.000000', NULL, 'Pengecekan stok gudang cabang', 7, 0, 1, 1, 7),
(12, 'LN-202611-0009', '2026-11-09 00:00:00.000000', '2026-11-16 00:00:00.000000', NULL, 'Kajian dokumen hukum dan perizinan', 7, 0, 1, 1, 8),
(13, 'LN-202611-0010', '2026-11-10 00:00:00.000000', '2026-11-17 00:00:00.000000', NULL, 'Riset pengembangan produk sistem inventory', 7, 0, 1, 1, 9),
(14, 'LN-202611-0011', '2026-11-11 00:00:00.000000', '2026-11-18 00:00:00.000000', NULL, 'Pengujian kualitas perangkat keras batch baru', 7, 0, 1, 1, 10),
(15, 'LN-202611-0012', '2026-11-12 00:00:00.000000', '2026-11-19 00:00:00.000000', NULL, 'Penanganan tiket komplain helpdesk', 7, 0, 1, 1, 11),
(16, 'LN-202611-0013', '2026-11-13 00:00:00.000000', '2026-11-20 00:00:00.000000', NULL, 'Survei vendor pengadaan barang', 7, 0, 1, 1, 12),
(17, 'LN-202611-0014', '2026-11-14 00:00:00.000000', '2026-11-21 00:00:00.000000', NULL, 'Peliputan kegiatan public relations', 7, 0, 1, 1, 13),
(18, 'LN-202611-0015', '2026-11-15 00:00:00.000000', '2026-11-22 00:00:00.000000', NULL, 'Perbaikan fasilitas ruang kantor', 7, 0, 1, 1, 14),
(19, 'LN-202611-0016', '2026-11-15 00:00:00.000000', '2026-11-22 00:00:00.000000', NULL, 'Pengamanan area parkir dan gerbang', 7, 0, 1, 1, 15),
(20, 'LN-202611-0017', '2026-11-15 00:00:00.000000', '2026-11-22 00:00:00.000000', NULL, 'Pelatihan internal sistem operasi', 7, 0, 1, 1, 16),
(21, 'LN-202611-0018', '2026-11-15 00:00:00.000000', '2026-11-22 00:00:00.000000', NULL, 'Audit internal triwulan', 7, 0, 1, 1, 17),
(22, 'LN-202611-0019', '2026-11-15 00:00:00.000000', '2026-11-22 00:00:00.000000', NULL, 'Persiapan event gathering tahunan', 7, 0, 1, 1, 18),
(23, 'LN-202611-0020', '2026-11-15 00:00:00.000000', '2026-11-22 00:00:00.000000', NULL, 'Administrasi kesekretariatan kantor', 7, 0, 1, 1, 19),
(24, 'LN-202611-0021', '2026-11-15 00:00:00.000000', '2026-11-22 00:00:00.000000', NULL, 'Penyusunan strategi bisnis jangka panjang', 7, 0, 1, 1, 20),
(25, 'LN-202611-0022', '2026-11-15 00:00:00.000000', '2026-11-22 00:00:00.000000', NULL, 'Konfigurasi server jaringan utama', 7, 0, 1, 1, 21),
(26, 'LN-202611-0023', '2026-11-15 00:00:00.000000', '2026-11-22 00:00:00.000000', NULL, 'Pengawasan operasional harian', 7, 0, 1, 1, 22),
(27, 'LN-202611-0024', '2026-11-15 00:00:00.000000', '2026-11-22 00:00:00.000000', NULL, 'Analisis laporan keuangan bulanan', 7, 0, 1, 1, 23),
(28, 'LN-202611-0025', '2026-11-15 00:00:00.000000', '2026-11-22 00:00:00.000000', NULL, 'Perekrutan dan wawancara kandidat', 7, 0, 1, 1, 24),
(29, 'LN-202611-0026', '2026-11-15 00:00:00.000000', '2026-11-22 00:00:00.000000', NULL, 'Kampanye pemasaran produk baru', 7, 0, 1, 1, 25),
(30, 'LN-202611-0027', '2026-11-15 00:00:00.000000', '2026-11-22 00:00:00.000000', NULL, 'Presentasi penawaran ke calon pelanggan', 7, 0, 1, 1, 26),
(31, 'LN-202611-0028', '2026-11-15 00:00:00.000000', '2026-11-22 00:00:00.000000', NULL, 'Pendataan inventaris gudang', 7, 0, 1, 1, 27),
(32, 'LN-202611-0029', '2026-11-15 00:00:00.000000', '2026-11-22 00:00:00.000000', NULL, 'Pemeriksaan kepatuhan SOP perusahaan', 7, 0, 1, 1, 28),
(33, 'LN-202611-0030', '2026-11-15 00:00:00.000000', '2026-11-22 00:00:00.000000', NULL, 'Pengujian fitur aplikasi web', 7, 0, 1, 1, 29),
(34, 'LN-202611-0031', '2026-11-15 00:00:00.000000', '2026-11-22 00:00:00.000000', NULL, 'Inspeksi mutu produk', 7, 0, 1, 1, 30),
(35, 'LN-202611-0032', '2026-11-15 00:00:00.000000', '2026-11-22 00:00:00.000000', NULL, 'Dukungan teknis helpdesk lapangan', 7, 0, 1, 1, 31),
(36, 'LN-202611-0033', '2026-11-15 00:00:00.000000', '2026-11-22 00:00:00.000000', NULL, 'Koordinasi dengan supplier utama', 7, 0, 1, 1, 32),
(37, 'LN-202611-0034', '2026-11-15 00:00:00.000000', '2026-11-22 00:00:00.000000', NULL, 'Konferensi pers dan hubungan media', 7, 0, 1, 1, 33),
(38, 'LN-202611-0035', '2026-11-15 00:00:00.000000', '2026-11-22 00:00:00.000000', NULL, 'Pemeliharaan fasilitas gedung', 7, 0, 1, 1, 34),
(39, 'LN-202611-0036', '2026-11-15 00:00:00.000000', '2026-11-22 00:00:00.000000', NULL, 'Patroli dan pengamanan area', 7, 0, 1, 1, 35),
(40, 'LN-202611-0037', '2026-11-15 00:00:00.000000', '2026-11-22 00:00:00.000000', NULL, 'Koordinasi program pelatihan staff', 7, 0, 1, 1, 36),
(41, 'LN-202611-0038', '2026-11-15 00:00:00.000000', '2026-11-22 00:00:00.000000', NULL, 'Pemeriksaan pembukuan keuangan', 7, 0, 1, 1, 37),
(42, 'LN-202611-0039', '2026-11-15 00:00:00.000000', '2026-11-22 00:00:00.000000', NULL, 'Pengelolaan acara perusahaan', 7, 0, 1, 1, 38),
(43, 'LN-202611-0040', '2026-11-15 00:00:00.000000', '2026-11-22 00:00:00.000000', NULL, 'Administrasi perkantoran harian', 7, 0, 1, 1, 39),
(44, 'LN-202611-0041', '2026-11-15 00:00:00.000000', '2026-11-22 00:00:00.000000', NULL, 'Analisis data performa bisnis', 7, 0, 1, 1, 40),
(45, 'LN-202611-0042', '2026-11-15 00:00:00.000000', '2026-11-22 00:00:00.000000', NULL, 'Perawatan infrastruktur jaringan', 7, 0, 1, 1, 41),
(46, 'LN-202611-0043', '2026-11-15 00:00:00.000000', '2026-11-22 00:00:00.000000', NULL, 'Pengelolaan administrasi operasional', 7, 0, 1, 1, 42),
(47, 'LN-202611-0044', '2026-11-15 00:00:00.000000', '2026-11-22 00:00:00.000000', NULL, 'Penghitungan dan pelaporan pajak', 7, 0, 1, 1, 43),
(48, 'LN-202611-0045', '2026-11-15 00:00:00.000000', '2026-11-22 00:00:00.000000', NULL, 'Pengelolaan data kepegawaian', 7, 0, 1, 1, 44),
(49, 'LN-202611-0046', '2026-11-15 00:00:00.000000', '2026-11-22 00:00:00.000000', NULL, 'Pembuatan materi publikasi digital', 7, 0, 1, 1, 45),
(50, 'LN-202611-0047', '2026-11-15 00:00:00.000000', '2026-11-22 00:00:00.000000', NULL, 'Pengelolaan administrasi penjualan', 7, 0, 1, 1, 46),
(51, 'LN-202611-0048', '2026-11-15 00:00:00.000000', '2026-11-22 00:00:00.000000', NULL, 'Pencatatan persediaan barang gudang', 7, 0, 1, 1, 47),
(52, 'LN-202611-0049', '2026-11-15 00:00:00.000000', '2026-11-22 00:00:00.000000', NULL, 'Pendampingan urusan hukum perusahaan', 7, 0, 1, 1, 48),
(53, 'LN-202611-0050', '2026-11-15 00:00:00.000000', '2026-11-22 00:00:00.000000', NULL, 'Praktikum riset dan pengujian lab', 7, 0, 1, 1, 49),
(54, 'LN-202611-0051', '2026-11-15 00:00:00.000000', '2026-11-22 00:00:00.000000', NULL, 'Audit standar kualitas operasional', 7, 0, 1, 1, 50);

-- --------------------------------------------------------

--
-- Table structure for table `peminjamanasetdetail`
--

CREATE TABLE `peminjamanasetdetail` (
  `Id` int(11) NOT NULL,
  `PeminjamanAsetID` int(11) NOT NULL,
  `ProdukItemId` int(11) NOT NULL,
  `NamaBarang` varchar(150) NOT NULL,
  `SerialNumber` varchar(100) NOT NULL,
  `LokasiAwal` varchar(100) NOT NULL,
  `Kuantitas` int(11) NOT NULL,
  `KondisiSerah` varchar(50) NOT NULL,
  `KondisiKembali` varchar(50) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `peminjamanasetdetail`
--

INSERT INTO `peminjamanasetdetail` (`Id`, `PeminjamanAsetID`, `ProdukItemId`, `NamaBarang`, `SerialNumber`, `LokasiAwal`, `Kuantitas`, `KondisiSerah`, `KondisiKembali`) VALUES
(1, 1, 2, 'Laptop Dell Latitude 5440 i7', 'ITE-0001-GDG-JKT-1-0002', 'Gudang Utama Jakarta', 1, 'Good', 'Broken'),
(2, 2, 2, 'Laptop Dell Latitude 5440 i7', 'ITE-0001-GDG-JKT-1-0002', 'Gudang Utama Jakarta', 1, 'Good', 'Good'),
(3, 3, 1, 'Laptop Dell Latitude 5440 i7', 'ITE-0001-GDG-JKT-1-0001', 'Gudang Utama Jakarta', 1, 'Good', NULL),
(4, 4, 4, 'Laptop Lenovo ThinkPad L14', 'ITE-0002-GDG-JKT-1-0002', 'Gudang Utama Jakarta', 1, 'Good', NULL),
(5, 5, 1, 'Laptop Dell Latitude 5440 i7', 'ITE-0001-GDG-JKT-1-0001', 'Gudang Utama Jakarta', 1, 'Good', 'Good'),
(6, 6, 2, 'Laptop Dell Latitude 5440 i7', 'ITE-0001-GDG-JKT-1-0002', 'Gudang Utama Jakarta', 1, 'Good', NULL),
(7, 7, 4, 'Laptop Lenovo ThinkPad L14', 'ITE-0002-GDG-JKT-1-0002', 'Gudang Utama Jakarta', 1, 'Good', 'Good'),
(8, 8, 6, 'Laptop Lenovo ThinkPad L14', 'ITE-0002-GDG-JKT-1-0004', 'Gudang Utama Jakarta', 1, 'Good', NULL),
(9, 9, 9, 'Laptop Dell Latitude 5440 i7', 'ITE-0003-GDG-SBY-01-0001', 'Gudang Cabang Surabaya', 1, 'Good', NULL),
(10, 10, 10, 'Laptop Dell Latitude 5440 i7', 'ITE-0003-GDG-SBY-01-0002', 'Gudang Cabang Surabaya', 1, 'Good', 'Good'),
(11, 11, 11, 'Laptop Dell Latitude 5440 i7', 'ITE-0003-GDG-SBY-01-0003', 'Gudang Cabang Surabaya', 1, 'Good', NULL),
(12, 12, 12, 'Laptop Dell Latitude 5440 i7', 'ITE-0003-GDG-SBY-01-0004', 'Gudang Cabang Surabaya', 1, 'Good', NULL),
(13, 13, 13, 'Laptop Dell Latitude 5440 i7', 'ITE-0003-GDG-SBY-01-0005', 'Gudang Cabang Surabaya', 1, 'Good', NULL),
(14, 14, 14, 'Laptop Dell Latitude 5440 i7', 'ITE-0003-GDG-SBY-01-0006', 'Gudang Cabang Surabaya', 1, 'Good', NULL),
(15, 15, 15, 'Laptop Dell Latitude 5440 i7', 'ITE-0003-GDG-SBY-01-0007', 'Gudang Cabang Surabaya', 1, 'Good', NULL),
(16, 16, 16, 'Laptop Dell Latitude 5440 i7', 'ITE-0003-GDG-SBY-01-0008', 'Gudang Cabang Surabaya', 1, 'Good', NULL),
(17, 17, 17, 'Laptop Dell Latitude 5440 i7', 'ITE-0003-GDG-SBY-01-0009', 'Gudang Cabang Surabaya', 1, 'Good', NULL),
(18, 18, 18, 'Laptop Dell Latitude 5440 i7', 'ITE-0003-GDG-SBY-01-0010', 'Gudang Cabang Surabaya', 1, 'Good', NULL),
(19, 19, 19, 'Laptop Dell Latitude 5440 i7', 'ITE-0003-GDG-SBY-01-0011', 'Gudang Cabang Surabaya', 1, 'Good', NULL),
(20, 20, 20, 'Laptop Dell Latitude 5440 i7', 'ITE-0003-GDG-SBY-01-0012', 'Gudang Cabang Surabaya', 1, 'Good', NULL),
(21, 21, 21, 'Laptop Dell Latitude 5440 i7', 'ITE-0003-GDG-SBY-01-0013', 'Gudang Cabang Surabaya', 1, 'Good', NULL),
(22, 22, 22, 'Laptop Dell Latitude 5440 i7', 'ITE-0003-GDG-SBY-01-0014', 'Gudang Cabang Surabaya', 1, 'Good', NULL),
(23, 23, 23, 'Laptop Dell Latitude 5440 i7', 'ITE-0003-GDG-SBY-01-0015', 'Gudang Cabang Surabaya', 1, 'Good', NULL),
(24, 24, 24, 'Laptop Dell Latitude 5440 i7', 'ITE-0003-GDG-SBY-01-0016', 'Gudang Cabang Surabaya', 1, 'Good', NULL),
(25, 25, 25, 'Laptop Dell Latitude 5440 i7', 'ITE-0003-GDG-SBY-01-0017', 'Gudang Cabang Surabaya', 1, 'Good', NULL),
(26, 26, 26, 'Laptop Dell Latitude 5440 i7', 'ITE-0003-GDG-SBY-01-0018', 'Gudang Cabang Surabaya', 1, 'Good', NULL),
(27, 27, 27, 'Laptop Dell Latitude 5440 i7', 'ITE-0003-GDG-SBY-01-0019', 'Gudang Cabang Surabaya', 1, 'Good', NULL),
(28, 28, 28, 'Laptop Dell Latitude 5440 i7', 'ITE-0003-GDG-SBY-01-0020', 'Gudang Cabang Surabaya', 1, 'Good', NULL),
(29, 29, 1, 'Laptop Dell Latitude 5440 i7', 'ITE-0001-GDG-JKT-1-0001', 'Gudang Utama Jakarta', 1, 'Good', NULL),
(30, 30, 2, 'Laptop Dell Latitude 5440 i7', 'ITE-0001-GDG-JKT-1-0002', 'Gudang Utama Jakarta', 1, 'Good', NULL),
(31, 31, 4, 'Laptop Lenovo ThinkPad L14', 'ITE-0002-GDG-JKT-1-0002', 'Gudang Utama Jakarta', 1, 'Good', NULL),
(32, 32, 6, 'Laptop Lenovo ThinkPad L14', 'ITE-0002-GDG-JKT-1-0004', 'Gudang Utama Jakarta', 1, 'Good', NULL),
(33, 33, 7, 'Laptop Lenovo ThinkPad L14', 'ITE-0002-GDG-JKT-1-0005', 'Gudang Utama Jakarta', 1, 'Good', NULL),
(34, 34, 9, 'Laptop Dell Latitude 5440 i7', 'ITE-0003-GDG-SBY-01-0001', 'Gudang Cabang Surabaya', 1, 'Good', NULL),
(35, 35, 10, 'Laptop Dell Latitude 5440 i7', 'ITE-0003-GDG-SBY-01-0002', 'Gudang Cabang Surabaya', 1, 'Good', NULL),
(36, 36, 11, 'Laptop Dell Latitude 5440 i7', 'ITE-0003-GDG-SBY-01-0003', 'Gudang Cabang Surabaya', 1, 'Good', NULL),
(37, 37, 12, 'Laptop Dell Latitude 5440 i7', 'ITE-0003-GDG-SBY-01-0004', 'Gudang Cabang Surabaya', 1, 'Good', NULL),
(38, 38, 13, 'Laptop Dell Latitude 5440 i7', 'ITE-0003-GDG-SBY-01-0005', 'Gudang Cabang Surabaya', 1, 'Good', NULL),
(39, 39, 14, 'Laptop Dell Latitude 5440 i7', 'ITE-0003-GDG-SBY-01-0006', 'Gudang Cabang Surabaya', 1, 'Good', NULL),
(40, 40, 15, 'Laptop Dell Latitude 5440 i7', 'ITE-0003-GDG-SBY-01-0007', 'Gudang Cabang Surabaya', 1, 'Good', NULL),
(41, 41, 16, 'Laptop Dell Latitude 5440 i7', 'ITE-0003-GDG-SBY-01-0008', 'Gudang Cabang Surabaya', 1, 'Good', NULL),
(42, 42, 17, 'Laptop Dell Latitude 5440 i7', 'ITE-0003-GDG-SBY-01-0009', 'Gudang Cabang Surabaya', 1, 'Good', NULL),
(43, 43, 18, 'Laptop Dell Latitude 5440 i7', 'ITE-0003-GDG-SBY-01-0010', 'Gudang Cabang Surabaya', 1, 'Good', NULL),
(44, 44, 19, 'Laptop Dell Latitude 5440 i7', 'ITE-0003-GDG-SBY-01-0011', 'Gudang Cabang Surabaya', 1, 'Good', NULL),
(45, 45, 20, 'Laptop Dell Latitude 5440 i7', 'ITE-0003-GDG-SBY-01-0012', 'Gudang Cabang Surabaya', 1, 'Good', NULL),
(46, 46, 21, 'Laptop Dell Latitude 5440 i7', 'ITE-0003-GDG-SBY-01-0013', 'Gudang Cabang Surabaya', 1, 'Good', NULL),
(47, 47, 22, 'Laptop Dell Latitude 5440 i7', 'ITE-0003-GDG-SBY-01-0014', 'Gudang Cabang Surabaya', 1, 'Good', NULL),
(48, 48, 23, 'Laptop Dell Latitude 5440 i7', 'ITE-0003-GDG-SBY-01-0015', 'Gudang Cabang Surabaya', 1, 'Good', NULL),
(49, 49, 24, 'Laptop Dell Latitude 5440 i7', 'ITE-0003-GDG-SBY-01-0016', 'Gudang Cabang Surabaya', 1, 'Good', NULL),
(50, 50, 25, 'Laptop Dell Latitude 5440 i7', 'ITE-0003-GDG-SBY-01-0017', 'Gudang Cabang Surabaya', 1, 'Good', NULL),
(51, 51, 26, 'Laptop Dell Latitude 5440 i7', 'ITE-0003-GDG-SBY-01-0018', 'Gudang Cabang Surabaya', 1, 'Good', NULL),
(52, 52, 27, 'Laptop Dell Latitude 5440 i7', 'ITE-0003-GDG-SBY-01-0019', 'Gudang Cabang Surabaya', 1, 'Good', NULL),
(53, 53, 28, 'Laptop Dell Latitude 5440 i7', 'ITE-0003-GDG-SBY-01-0020', 'Gudang Cabang Surabaya', 1, 'Good', NULL),
(54, 54, 4, 'Laptop Lenovo ThinkPad L14', 'ITE-0002-GDG-JKT-1-0002', 'Gudang Utama Jakarta', 1, 'Good', NULL);

-- --------------------------------------------------------

--
-- Table structure for table `penugasanasetdetail`
--

CREATE TABLE `penugasanasetdetail` (
  `Id` int(11) NOT NULL,
  `PenugasanAsetId` int(11) NOT NULL,
  `NamaBarang` varchar(100) NOT NULL,
  `SerialNumber` varchar(100) DEFAULT NULL,
  `LokasiAwal` varchar(100) DEFAULT NULL,
  `Kuantitas` int(11) NOT NULL,
  `KondisiSerah` varchar(50) NOT NULL,
  `KondisiKembali` varchar(50) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `penugasanasetdetail`
--

INSERT INTO `penugasanasetdetail` (`Id`, `PenugasanAsetId`, `NamaBarang`, `SerialNumber`, `LokasiAwal`, `Kuantitas`, `KondisiSerah`, `KondisiKembali`) VALUES
(1, 1, 'Laptop Dell Latitude 5440 i7', 'ITE-0001-GDG-JKT-1-0001', 'Gudang Utama Jakarta', 1, 'Good', 'Good'),
(2, 2, 'Laptop Dell Latitude 5440 i7', 'ITE-0001-GDG-JKT-1-0001', 'Gudang Utama Jakarta', 1, 'Good', 'Lost'),
(3, 3, 'Laptop Dell Latitude 5440 i7', 'ITE-0001-GDG-JKT-1-0001', 'Gudang Utama Jakarta', 1, 'Good', NULL),
(4, 4, 'Laptop Dell Latitude 5440 i7', 'ITE-0001-GDG-JKT-1-0001', 'Gudang Utama Jakarta', 1, 'Good', 'Broken'),
(5, 5, 'Laptop Dell Latitude 5440 i7', 'ITE-0001-GDG-JKT-1-0001', 'Gudang Utama Jakarta', 1, 'Good', 'Good'),
(6, 6, 'Laptop Dell Latitude 5440 i7', 'ITE-0001-GDG-JKT-1-0002', 'Gudang Utama Jakarta', 1, 'Good', 'Good'),
(7, 7, 'Laptop Dell Latitude 5440 i7', 'ITE-0001-GDG-JKT-1-0001', 'Gudang Utama Jakarta', 1, 'Good', 'Good'),
(8, 8, 'Laptop Dell Latitude 5440 i7', 'ITE-0001-GDG-JKT-1-0001', 'Gudang Utama Jakarta', 1, 'Good', 'Good'),
(9, 9, 'Laptop Dell Latitude 5440 i7', 'ITE-0001-GDG-JKT-1-0001', 'Gudang Utama Jakarta', 1, 'Good', 'Good'),
(10, 10, 'Laptop Dell Latitude 5440 i7', 'ITE-0001-GDG-JKT-1-0002', 'Gudang Utama Jakarta', 1, 'Good', 'Good'),
(11, 11, 'Laptop Dell Latitude 5440 i7', 'ITE-0001-GDG-JKT-1-0001', 'Gudang Utama Jakarta', 1, 'Good', 'Good'),
(12, 12, 'Laptop Dell Latitude 5440 i7', 'ITE-0001-GDG-JKT-1-0001', 'Gudang Utama Jakarta', 1, 'Good', NULL),
(13, 13, 'Laptop Dell Latitude 5440 i7', 'ITE-0001-GDG-JKT-1-0001', 'Transfer Langsung', 1, 'Good', 'Good'),
(14, 14, 'Laptop Dell Latitude 5440 i7', 'ITE-0001-GDG-JKT-1-0002', 'Gudang Utama Jakarta', 1, 'Good', NULL),
(15, 15, 'Laptop Dell Latitude 5440 i7', 'ITE-0001-GDG-JKT-1-0002', 'Transfer Langsung', 1, 'Good', 'Good'),
(16, 16, 'Laptop Lenovo ThinkPad L14', 'ITE-0002-GDG-JKT-1-0002', 'Gudang Utama Jakarta', 1, 'Good', 'Good'),
(17, 17, 'Laptop Lenovo ThinkPad L14', 'ITE-0002-GDG-JKT-1-0003', 'Gudang Utama Jakarta', 1, 'Good', 'Lost'),
(18, 18, 'Laptop Lenovo ThinkPad L14', 'ITE-0002-GDG-JKT-1-0002', 'Gudang Utama Jakarta', 1, 'Good', 'Good'),
(19, 19, 'Laptop Dell Latitude 5440 i7', 'ITE-0001-GDG-JKT-1-0001', 'Gudang Utama Jakarta', 1, 'Good', 'Good'),
(20, 20, 'Laptop Dell Latitude 5440 i7', 'ITE-0001-GDG-JKT-1-0001', 'Gudang Utama Jakarta', 1, 'Good', NULL),
(21, 21, 'XYZ', 'NON-SN', 'Gudang Cabang Samarinda', 1, 'Good', 'Good'),
(22, 22, 'XYZ', 'NON-SN', 'Gudang Cabang Samarinda', 2, 'Good', NULL),
(23, 23, 'Laptop ASUS Zenbook 14 OLED', 'ITE-0006-GDG-JKT-1-0001', 'Gudang Utama Jakarta', 1, 'Good', NULL),
(24, 24, 'PC Desktop HP ProDesk 400 G9', 'ITE-0007-GDG-JKT-1-0001', 'Gudang Utama Jakarta', 1, 'Good', NULL),
(25, 25, 'Laptop Dell Latitude 5440 i7', 'ITE-0001-GDG-JKT-1-0003', 'Gudang Utama Jakarta', 1, 'Good', NULL),
(26, 26, 'Laptop Lenovo ThinkPad L14', 'ITE-0002-GDG-JKT-1-0005', 'Gudang Utama Jakarta', 1, 'Good', NULL),
(27, 27, 'Cisco Managed Switch 24-Port', 'NET-0001-GDG-JKT-1-0001', 'Gudang Utama Jakarta', 1, 'Good', NULL),
(28, 28, 'Router MikroTik hEX RB750Gr3', 'NET-0002-GDG-JKT-1-0001', 'Gudang Utama Jakarta', 1, 'Good', NULL),
(29, 29, 'Laptop ASUS Zenbook 14 OLED', 'ITE-0006-GDG-JKT-1-0002', 'Gudang Utama Jakarta', 1, 'Good', NULL),
(30, 30, 'PC Desktop HP ProDesk 400 G9', 'ITE-0007-GDG-JKT-1-0002', 'Gudang Utama Jakarta', 1, 'Good', NULL),
(31, 31, 'IP Phone Grandstream GXP1625', 'COM-0001-GDG-JKT-1-0001', 'Gudang Utama Jakarta', 1, 'Good', NULL),
(32, 32, 'Printer Barcode Thermal TSC TE244', 'HWD-0001-GDG-JKT-1-0001', 'Gudang Utama Jakarta', 1, 'Good', NULL),
(33, 33, 'Barcode Scanner Wireless Omni', 'HWD-0002-GDG-JKT-1-0001', 'Gudang Utama Jakarta', 1, 'Good', NULL),
(34, 34, 'Infocus Projector 3800 Lumens', 'AV-0001-GDG-JKT-1-0001', 'Gudang Utama Jakarta', 1, 'Good', NULL),
(35, 35, 'IP Camera CCTV Outdoor 2MP', 'SEC-0001-GDG-JKT-1-0001', 'Gudang Utama Jakarta', 1, 'Good', NULL),
(36, 36, 'Digital Smart Door Lock Biometric', 'SEC-0002-GDG-JKT-1-0001', 'Gudang Utama Jakarta', 1, 'Good', NULL),
(37, 37, 'Digital Multimeter Fluke 101', 'TST-0001-GDG-JKT-1-0001', 'Gudang Utama Jakarta', 1, 'Good', NULL),
(38, 38, 'Speaker Portable Conference Meeting', 'AV-0003-GDG-JKT-1-0001', 'Gudang Utama Jakarta', 1, 'Good', NULL),
(39, 39, 'Laptop Dell Latitude 5440 i7', 'ITE-0001-GDG-JKT-1-0003', 'Gudang Utama Jakarta', 1, 'Good', NULL),
(40, 40, 'Monitor Dell 24 Inch IPS', 'ITE-0008-GDG-JKT-1-0001', 'Gudang Utama Jakarta', 1, 'Good', NULL),
(41, 41, 'UPS APC Smart-UPS 1500VA', 'SRV-0002-GDG-JKT-1-0001', 'Gudang Utama Jakarta', 1, 'Good', NULL),
(42, 42, 'Metal Detector Handheld Security', 'SEC-0003-GDG-JKT-1-0001', 'Gudang Utama Jakarta', 1, 'Good', NULL),
(43, 43, 'External Hard Disk 2TB USB 3.0', 'ITE-0010-GDG-JKT-1-0001', 'Gudang Utama Jakarta', 1, 'Good', NULL),
(44, 44, 'Wi-Fi 6 Access Point TP-Link EAP610', 'NET-0003-GDG-JKT-1-0001', 'Gudang Utama Jakarta', 1, 'Good', NULL),
(45, 45, 'Tablet Samsung Galaxy Tab A9', 'ITE-0011-GDG-JKT-1-0001', 'Gudang Utama Jakarta', 1, 'Good', NULL),
(46, 46, 'Laptop ASUS Zenbook 14 OLED', 'ITE-0006-GDG-JKT-1-0003', 'Gudang Utama Jakarta', 1, 'Good', NULL),
(47, 47, 'PC Desktop HP ProDesk 400 G9', 'ITE-0007-GDG-JKT-1-0003', 'Gudang Utama Jakarta', 1, 'Good', NULL),
(48, 48, 'Laptop Lenovo ThinkPad L14', 'ITE-0002-GDG-JKT-1-0005', 'Gudang Utama Jakarta', 1, 'Good', NULL),
(49, 49, 'Digital PH Meter Benchtop', 'LAB-0001-GDG-JKT-1-0001', 'Gudang Utama Jakarta', 1, 'Good', NULL),
(50, 50, 'Motor Operasional Honda Beat CBS', 'VEH-0001-GDG-JKT-1-0001', 'Gudang Utama Jakarta', 1, 'Good', NULL),
(51, 51, 'IP Phone Grandstream GXP1625', 'COM-0001-GDG-JKT-1-0002', 'Gudang Utama Jakarta', 1, 'Good', NULL),
(52, 52, 'Printer Barcode Thermal TSC TE244', 'HWD-0001-GDG-JKT-1-0002', 'Gudang Utama Jakarta', 1, 'Good', NULL),
(53, 53, 'Barcode Scanner Wireless Omni', 'HWD-0002-GDG-JKT-1-0002', 'Gudang Utama Jakarta', 1, 'Good', NULL),
(54, 54, 'Infocus Projector 3800 Lumens', 'AV-0001-GDG-JKT-1-0002', 'Gudang Utama Jakarta', 1, 'Good', NULL),
(55, 55, 'IP Camera CCTV Outdoor 2MP', 'SEC-0001-GDG-JKT-1-0002', 'Gudang Utama Jakarta', 1, 'Good', NULL),
(56, 56, 'Digital Smart Door Lock Biometric', 'SEC-0002-GDG-JKT-1-0002', 'Gudang Utama Jakarta', 1, 'Good', NULL),
(57, 57, 'Digital Multimeter Fluke 101', 'TST-0001-GDG-JKT-1-0002', 'Gudang Utama Jakarta', 1, 'Good', NULL),
(58, 58, 'Speaker Portable Conference Meeting', 'AV-0003-GDG-JKT-1-0002', 'Gudang Utama Jakarta', 1, 'Good', NULL),
(59, 59, 'Monitor Dell 24 Inch IPS', 'ITE-0008-GDG-JKT-1-0002', 'Gudang Utama Jakarta', 1, 'Good', NULL),
(60, 60, 'UPS APC Smart-UPS 1500VA', 'SRV-0002-GDG-JKT-1-0002', 'Gudang Utama Jakarta', 1, 'Good', NULL),
(61, 61, 'Cisco Managed Switch 24-Port', 'NET-0001-GDG-JKT-1-0002', 'Gudang Utama Jakarta', 1, 'Good', NULL),
(62, 62, 'Router MikroTik hEX RB750Gr3', 'NET-0002-GDG-JKT-1-0002', 'Gudang Utama Jakarta', 1, 'Good', NULL),
(63, 63, 'Laptop ASUS Zenbook 14 OLED', 'ITE-0006-GDG-JKT-1-0004', 'Gudang Utama Jakarta', 1, 'Good', NULL),
(64, 64, 'PC Desktop HP ProDesk 400 G9', 'ITE-0007-GDG-JKT-1-0004', 'Gudang Utama Jakarta', 1, 'Good', NULL),
(65, 65, 'External Hard Disk 2TB USB 3.0', 'ITE-0010-GDG-JKT-1-0002', 'Gudang Utama Jakarta', 1, 'Good', NULL),
(66, 66, 'Wi-Fi 6 Access Point TP-Link EAP610', 'NET-0003-GDG-JKT-1-0002', 'Gudang Utama Jakarta', 1, 'Good', NULL),
(67, 67, 'Tablet Samsung Galaxy Tab A9', 'ITE-0011-GDG-JKT-1-0002', 'Gudang Utama Jakarta', 1, 'Good', NULL),
(68, 68, 'Metal Detector Handheld Security', 'SEC-0003-GDG-JKT-1-0002', 'Gudang Utama Jakarta', 1, 'Good', NULL),
(69, 69, 'Digital PH Meter Benchtop', 'LAB-0001-GDG-JKT-1-0002', 'Gudang Utama Jakarta', 1, 'Good', NULL),
(70, 70, 'Motor Operasional Honda Beat CBS', 'VEH-0001-GDG-JKT-1-0002', 'Gudang Utama Jakarta', 1, 'Good', NULL);

-- --------------------------------------------------------

--
-- Table structure for table `penugasanasets`
--

CREATE TABLE `penugasanasets` (
  `Id` int(11) NOT NULL,
  `KodePenugasan` varchar(50) NOT NULL,
  `TipePenerima` int(11) NOT NULL,
  `TargetPenerimaId` int(11) NOT NULL,
  `TanggalSerahTerima` datetime(6) NOT NULL,
  `TanggalPengembalian` datetime(6) DEFAULT NULL,
  `Keterangan` varchar(1000) DEFAULT NULL,
  `Status` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `penugasanasets`
--

INSERT INTO `penugasanasets` (`Id`, `KodePenugasan`, `TipePenerima`, `TargetPenerimaId`, `TanggalSerahTerima`, `TanggalPengembalian`, `Keterangan`, `Status`) VALUES
(1, 'AS-202610-0001', 1, 1, '2026-10-30 00:00:00.000000', '2026-10-30 00:00:00.000000', 'Penugasan asset baru ke karyawan baru.', 3),
(2, 'AS-202610-0002', 2, 1, '2026-10-31 00:00:00.000000', '2026-10-30 00:00:00.000000', NULL, 3),
(3, 'AS-202610-0003', 3, 1, '2026-10-31 00:00:00.000000', NULL, NULL, 4),
(4, 'AS-202610-0004', 1, 1, '2026-10-31 00:00:00.000000', '2026-11-05 00:00:00.000000', NULL, 3),
(5, 'AS-202610-0005', 1, 1, '2026-10-31 00:00:00.000000', '2026-11-01 00:00:00.000000', NULL, 3),
(6, 'AS-202610-0006', 2, 1, '2026-10-31 00:00:00.000000', '2026-11-01 00:00:00.000000', NULL, 3),
(7, 'AS-202610-0007', 3, 1, '2026-10-31 00:00:00.000000', '2026-11-08 00:00:00.000000', NULL, 3),
(8, 'AS-202610-0008', 1, 1, '2026-10-31 00:00:00.000000', '2026-10-31 00:00:00.000000', NULL, 3),
(9, 'AS-202610-0009', 1, 1, '2026-10-31 00:00:00.000000', '2026-11-01 00:00:00.000000', NULL, 3),
(10, 'AS-202610-0010', 2, 1, '2026-10-31 00:00:00.000000', '2026-11-07 00:00:00.000000', NULL, 3),
(11, 'AS-202610-0011', 3, 1, '2026-10-31 00:00:00.000000', '2026-11-06 00:00:00.000000', NULL, 3),
(12, 'AS-202610-0012', 1, 1, '2026-10-31 00:00:00.000000', '2026-10-31 00:00:00.000000', NULL, 3),
(13, 'AS-202610-0013', 1, 2, '2026-10-31 00:00:00.000000', '2026-11-08 00:00:00.000000', '[Transfer] Siklus: Dari \'Andi Setiawan\' ke \'Anto\'. Asset di transfer ke karyawan lain a/n : Anto', 3),
(14, 'AS-202610-0014', 1, 1, '2026-10-31 00:00:00.000000', '2026-10-31 00:00:00.000000', NULL, 3),
(15, 'AS-202610-0015', 1, 2, '2026-10-31 00:00:00.000000', '2026-10-31 00:00:00.000000', '[Transfer] Siklus: Dari \'Andi Setiawan\' ke \'Anto\'. ', 3),
(16, 'AS-202610-0016', 1, 0, '2026-10-31 00:00:00.000000', '2026-11-01 00:00:00.000000', NULL, 3),
(17, 'AS-202610-0017', 2, 1, '2026-10-31 00:00:00.000000', '2026-11-01 00:00:00.000000', NULL, 3),
(18, 'AS-202610-0018', 1, 1, '2026-10-31 00:00:00.000000', '2026-11-08 00:00:00.000000', NULL, 3),
(19, 'AS-202611-0019', 1, 1, '2026-11-01 00:00:00.000000', '2026-11-15 00:00:00.000000', NULL, 3),
(20, 'AS-202611-0020', 1, 37, '2026-11-15 00:00:00.000000', NULL, NULL, 2),
(21, 'AS-202611-0021', 1, 3, '2026-11-16 00:00:00.000000', '2026-11-25 00:00:00.000000', NULL, 3),
(22, 'AS-202611-0022', 2, 13, '2026-11-16 00:00:00.000000', NULL, NULL, 2),
(23, 'AS-202611-0023', 2, 3, '2026-11-15 00:00:00.000000', '2026-11-25 00:00:00.000000', 'Peminjaman inventaris divisi keuangan.', 2),
(24, 'AS-202611-0024', 1, 4, '2026-11-15 00:00:00.000000', NULL, 'Penugasan aset HR specialist.', 2),
(25, 'AS-202611-0025', 1, 5, '2026-11-15 00:00:00.000000', '2026-11-30 00:00:00.000000', 'Penugasan marketing executive.', 2),
(26, 'AS-202611-0026', 1, 6, '2026-11-15 00:00:00.000000', NULL, 'Penugasan sales lapangan.', 2),
(27, 'AS-202611-0027', 1, 7, '2026-11-15 00:00:00.000000', '2026-12-01 00:00:00.000000', 'Penugasan logistik koordinator.', 2),
(28, 'AS-202611-0028', 1, 8, '2026-11-15 00:00:00.000000', NULL, 'Penugasan legal counsel.', 2),
(29, 'AS-202611-0029', 1, 9, '2026-11-15 00:00:00.000000', '2026-12-05 00:00:00.000000', 'Penugasan R&D researcher.', 2),
(30, 'AS-202611-0030', 1, 10, '2026-11-15 00:00:00.000000', NULL, 'Penugasan QA analyst.', 2),
(31, 'AS-202611-0031', 1, 11, '2026-11-15 00:00:00.000000', '2026-11-22 00:00:00.000000', 'Penugasan support agent.', 2),
(32, 'AS-202611-0032', 1, 12, '2026-11-15 00:00:00.000000', NULL, 'Penugasan procurement officer.', 2),
(33, 'AS-202611-0033', 1, 13, '2026-11-15 00:00:00.000000', '2026-11-28 00:00:00.000000', 'Penugasan PR specialist.', 2),
(34, 'AS-202611-0034', 1, 14, '2026-11-15 00:00:00.000000', NULL, 'Penugasan maintenance staff.', 2),
(35, 'AS-202611-0035', 1, 15, '2026-11-15 00:00:00.000000', '2026-12-10 00:00:00.000000', 'Penugasan security guard.', 2),
(36, 'AS-202611-0036', 1, 16, '2026-11-15 00:00:00.000000', NULL, 'Penugasan trainer.', 2),
(37, 'AS-202611-0037', 1, 17, '2026-11-15 00:00:00.000000', '2026-12-15 00:00:00.000000', 'Penugasan internal auditor.', 2),
(38, 'AS-202611-0038', 1, 18, '2026-11-15 00:00:00.000000', NULL, 'Penugasan event organizer.', 2),
(39, 'AS-202611-0039', 1, 19, '2026-11-15 00:00:00.000000', '2026-11-20 00:00:00.000000', 'Penugasan admin staff.', 2),
(40, 'AS-202611-0040', 1, 20, '2026-11-15 00:00:00.000000', NULL, 'Penugasan strategic planner.', 2),
(41, 'AS-202611-0041', 1, 21, '2026-11-15 00:00:00.000000', '2026-12-20 00:00:00.000000', 'Penugasan system administrator.', 2),
(42, 'AS-202611-0042', 1, 22, '2026-11-15 00:00:00.000000', NULL, 'Penugasan operations supervisor.', 2),
(43, 'AS-202611-0043', 1, 23, '2026-11-15 00:00:00.000000', '2026-11-28 00:00:00.000000', 'Penugasan financial analyst.', 2),
(44, 'AS-202611-0044', 1, 24, '2026-11-15 00:00:00.000000', NULL, 'Penugasan recruiter.', 2),
(45, 'AS-202611-0045', 1, 25, '2026-11-15 00:00:00.000000', '2026-12-05 00:00:00.000000', 'Penugasan brand manager.', 2),
(46, 'AS-202611-0046', 1, 26, '2026-11-15 00:00:00.000000', NULL, 'Penugasan sales executive.', 2),
(47, 'AS-202611-0047', 1, 27, '2026-11-15 00:00:00.000000', '2026-11-30 00:00:00.000000', 'Penugasan warehouse staff.', 2),
(48, 'AS-202611-0048', 1, 28, '2026-11-15 00:00:00.000000', NULL, 'Penugasan compliance officer.', 2),
(49, 'AS-202611-0049', 1, 29, '2026-11-15 00:00:00.000000', '2026-12-10 00:00:00.000000', 'Penugasan product tester.', 2),
(50, 'AS-202611-0050', 1, 30, '2026-11-15 00:00:00.000000', NULL, 'Penugasan quality control.', 2),
(51, 'AS-202611-0051', 1, 31, '2026-11-15 00:00:00.000000', '2026-11-25 00:00:00.000000', 'Penugasan helpdesk support.', 2),
(52, 'AS-202611-0052', 1, 32, '2026-11-15 00:00:00.000000', NULL, 'Penugasan vendor relations.', 2),
(53, 'AS-202611-0053', 1, 33, '2026-11-15 00:00:00.000000', '2026-12-05 00:00:00.000000', 'Penugasan media relations.', 2),
(54, 'AS-202611-0054', 1, 34, '2026-11-15 00:00:00.000000', NULL, 'Penugasan facility assistant.', 2),
(55, 'AS-202611-0055', 1, 35, '2026-11-15 00:00:00.000000', '2026-12-15 00:00:00.000000', 'Penugasan security supervisor.', 2),
(56, 'AS-202611-0056', 1, 36, '2026-11-15 00:00:00.000000', NULL, 'Penugasan training coordinator.', 2),
(57, 'AS-202611-0057', 1, 37, '2026-11-15 00:00:00.000000', '2026-12-20 00:00:00.000000', 'Penugasan junior auditor.', 2),
(58, 'AS-202611-0058', 1, 38, '2026-11-15 00:00:00.000000', NULL, 'Penugasan event coordinator.', 2),
(59, 'AS-202611-0059', 1, 39, '2026-11-15 00:00:00.000000', '2026-11-30 00:00:00.000000', 'Penugasan office assistant.', 2),
(60, 'AS-202611-0060', 1, 40, '2026-11-15 00:00:00.000000', NULL, 'Penugasan business analyst.', 2),
(61, 'AS-202611-0061', 1, 41, '2026-11-15 00:00:00.000000', '2026-12-10 00:00:00.000000', 'Penugasan network engineer.', 2),
(62, 'AS-202611-0062', 1, 42, '2026-11-15 00:00:00.000000', NULL, 'Penugasan operations admin.', 2),
(63, 'AS-202611-0063', 1, 43, '2026-11-15 00:00:00.000000', '2026-11-28 00:00:00.000000', 'Penugasan tax specialist.', 2),
(64, 'AS-202611-0064', 1, 44, '2026-11-15 00:00:00.000000', NULL, 'Penugasan HR admin.', 2),
(65, 'AS-202611-0065', 1, 45, '2026-11-15 00:00:00.000000', '2026-12-05 00:00:00.000000', 'Penugasan copywriter.', 2),
(66, 'AS-202611-0066', 1, 46, '2026-11-15 00:00:00.000000', NULL, 'Penugasan sales admin.', 2),
(67, 'AS-202611-0067', 1, 47, '2026-11-15 00:00:00.000000', '2026-12-15 00:00:00.000000', 'Penugasan inventory keeper.', 2),
(68, 'AS-202611-0068', 1, 48, '2026-11-15 00:00:00.000000', NULL, 'Penugasan legal assistant.', 2),
(69, 'AS-202611-0069', 1, 49, '2026-11-15 00:00:00.000000', '2026-12-20 00:00:00.000000', 'Penugasan lab assistant.', 2),
(70, 'AS-202611-0070', 1, 50, '2026-11-15 00:00:00.000000', NULL, 'Penugasan standard auditor.', 2);

-- --------------------------------------------------------

--
-- Table structure for table `permissions`
--

CREATE TABLE `permissions` (
  `Id` int(11) NOT NULL,
  `Name` varchar(100) NOT NULL,
  `Category` varchar(100) NOT NULL,
  `Type` varchar(50) NOT NULL,
  `Description` longtext DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `permissions`
--

INSERT INTO `permissions` (`Id`, `Name`, `Category`, `Type`, `Description`) VALUES
(1, 'view dashboard', 'Navigation & View', 'System Core', NULL),
(2, 'manage users', 'User Management', 'System Core', NULL),
(3, 'manage settings', 'System Settings', 'System Core', NULL),
(4, 'view logs', 'Audit Logs', 'System Core', NULL),
(5, 'manage warehouses', 'General', 'Custom', NULL),
(6, 'manage products', 'General', 'Custom', NULL),
(7, 'manage suppliers', 'General', 'Custom', NULL),
(8, 'manage stock opname', 'General', 'Custom', NULL),
(9, 'manage transfer orders', 'General', 'Custom', NULL),
(10, 'view stock matrix', 'Navigation & View', 'Custom', NULL),
(11, 'view stock cards', 'Navigation & View', 'Custom', NULL),
(12, 'manage departments', 'General', 'Custom', NULL),
(13, 'manage employees', 'General', 'Custom', NULL),
(14, 'manage assignments', 'General', 'Custom', NULL),
(15, 'manage loans', 'General', 'Custom', NULL),
(16, 'view user guide', 'Navigation & View', 'Custom', NULL),
(17, 'view reparasi asset', 'General', 'Custom', NULL),
(18, 'manage customer', 'General', 'Custom', NULL),
(19, 'manage categories', 'General', 'Custom', NULL),
(20, 'view asset reports', 'General', 'Custom', NULL),
(21, 'view stock in/out reports', 'General', 'Custom', NULL),
(23, 'manage uom', 'General', 'Custom', NULL),
(24, 'manage racks', 'General', 'Custom', NULL),
(25, 'manage reparasi aset', 'General', 'Custom', NULL),
(26, 'manage roles and permissions', 'System Settings', 'System Core', NULL),
(27, 'manage stock transactions', 'General', 'Custom', NULL);

-- --------------------------------------------------------

--
-- Table structure for table `produk`
--

CREATE TABLE `produk` (
  `Id` int(11) NOT NULL,
  `PelacakanBarang` longtext NOT NULL,
  `Sku` varchar(50) NOT NULL,
  `NamaProduk` varchar(200) NOT NULL,
  `Kategori` varchar(100) NOT NULL,
  `Merek` varchar(100) DEFAULT NULL,
  `Satuan` varchar(50) NOT NULL,
  `TipeBarang` varchar(100) NOT NULL,
  `HargaBeli` decimal(18,2) NOT NULL,
  `HargaPerolehan` decimal(18,2) NOT NULL,
  `Deskripsi` longtext DEFAULT NULL,
  `Stok` int(11) NOT NULL,
  `GudangId` int(11) NOT NULL,
  `RakId` int(11) DEFAULT NULL,
  `MasterUomId` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `produk`
--

INSERT INTO `produk` (`Id`, `PelacakanBarang`, `Sku`, `NamaProduk`, `Kategori`, `Merek`, `Satuan`, `TipeBarang`, `HargaBeli`, `HargaPerolehan`, `Deskripsi`, `Stok`, `GudangId`, `RakId`, `MasterUomId`) VALUES
(1, 'Pelacakan Serial Number (SN)', 'SKU-ITE-0001', 'Laptop Dell Latitude 5440 i7', 'IT Equipment', 'Dell', 'Unit', 'Asset', 16500000.00, 16500000.00, 'Laptop Dell Latitude 5440, Intel Core i7, 16GB RAM, 512GB SSD.', 0, 1, 1, NULL),
(2, 'Pelacakan Serial Number (SN)', 'SKU-ITE-0002', 'Laptop Lenovo ThinkPad L14', 'IT Equipment', 'Lenovo', 'Unit', 'Inventory (Komersial)', 14500000.00, 14500000.00, 'Laptop Lenovo ThinkPad L14 Gen 4, AMD Ryzen 5, 16GB RAM, 512GB SSD.', 1, 1, 1, NULL),
(3, 'Tanpa Serial Number (Bulk)', 'SKU-CON-0001', 'Kertas A4 80gsm PaperOne', 'Consumables', '-', 'Box', 'Consumable (Habis Pakai)', 245000.00, 245000.00, 'Photocopy paper A4 size 80gsm, 1 box = 5 reams.', 10, 1, 1, NULL),
(5, 'Pelacakan Serial Number (SN)', 'SKU-ITE-0002', 'Laptop Lenovo ThinkPad L14', 'IT Equipment', 'Lenovo', 'Unit', 'Inventory (Komersial)', 14500000.00, 14500000.00, 'Laptop Lenovo ThinkPad L14 Gen 4, AMD Ryzen 5, 16GB RAM, 512GB SSD.', 1, 2, 1, NULL),
(6, 'Pelacakan Serial Number (SN)', 'SKU-ITE-0003', 'Laptop Dell Latitude 5440 i7', 'IT Equipment', 'Dell', 'Unit', 'Asset', 16500000.00, 16500000.00, 'Laptop Dell Latitude 5440, Intel Core i7, 16GB RAM, 512GB SSD.', 20, 2, 2, NULL),
(7, 'Tanpa Serial Number (Bulk)', 'SKU-ITE-0004', 'Laptop Dell Latitude 5440 i7', 'IT Equipment', 'Dell', 'Unit', 'Inventory (Komersial)', 15600000.00, 15600000.00, 'Laptop Dell Latitude 5440, Intel Core i7, 16GB RAM, 512GB SSD.', 15, 3, NULL, NULL),
(9, 'Tanpa Serial Number (Bulk)', 'SKU-ITE-0005', 'Laptop Dell Latitude 5440 i7', 'IT Equipment', 'Dell', 'Unit', 'Inventory (Komersial)', 15600000.00, 15600000.00, '-', 20, 1, 1, 2),
(10, 'Tanpa Serial Number (Bulk)', 'SKU-ITE-0006', 'XYZ', 'IT Equipment', 'XYX', 'Pieces', 'Inventory (Komersial)', 1.00, 1.00, 'XYZ', 0, 4, 3, 1),
(11, 'Pelacakan Serial Number (SN)', 'SKU-ITE-0007', 'PC Desktop HP ProDesk 400 G9', 'IT Equipment', 'HP', 'Unit', 'Asset', 8500000.00, 8500000.00, 'HP ProDesk 400 G9, Intel Core i5, 8GB RAM, 512GB SSD.', 10, 1, 3, 2),
(12, 'Tanpa Serial Number (Bulk)', 'SKU-CON-0002', 'Kertas HVS F4 Sinar Dunia 70gsm', 'Consumables', 'Sinar Dunia', 'Box', 'Consumable (Habis Pakai)', 210000.00, 210000.00, 'Kertas F4 70gsm, 1 box isi 5 rim.', 25, 1, 5, 4),
(13, 'Tanpa Serial Number (Bulk)', 'SKU-CON-0003', 'Tinta Printer Epson 003 Black', 'Consumables', 'Epson', 'Bottle', 'Consumable (Habis Pakai)', 95000.00, 95000.00, 'Original Epson 003 Ink Bottle Black 65ml.', 50, 1, 5, 1),
(14, 'Tanpa Serial Number (Bulk)', 'SKU-OFF-0001', 'Ballpoint Standard AE7 Black', 'Office Supplies', 'Standard', 'Box', 'Consumable (Habis Pakai)', 25000.00, 25000.00, 'Pulpen Standard AE7 hitam 1 box isi 12 pcs.', 30, 1, 5, 4),
(15, 'Tanpa Serial Number (Bulk)', 'SKU-OFF-0002', 'Buku Agenda Kerja Kulit', 'Office Supplies', 'Paperline', 'Pcs', 'Consumable (Habis Pakai)', 35000.00, 35000.00, 'Buku catatan harian / agenda cover kulit sintesis.', 40, 1, 5, 1),
(16, 'Pelacakan Serial Number (SN)', 'SKU-NET-0001', 'Cisco Managed Switch 24-Port', 'Networking Devices', 'Cisco', 'Unit', 'Asset', 4500000.00, 4500000.00, 'Cisco Catalyst 24-Port Gigabit Managed Switch.', 4, 1, 6, 2),
(17, 'Pelacakan Serial Number (SN)', 'SKU-NET-0002', 'Router MikroTik hEX RB750Gr3', 'Networking Devices', 'MikroTik', 'Unit', 'Asset', 950000.00, 950000.00, 'MikroTik hEX Gigabit Ethernet Router.', 8, 1, 6, 2),
(18, 'Pelacakan Serial Number (SN)', 'SKU-SRV-0001', 'Rack Server 19 Inch 20U Perforated', 'Server & Storage', 'Apacer', 'Unit', 'Asset', 5500000.00, 5500000.00, 'Standing server rack 20U lengkap dengan kipas dan PDU.', 2, 1, 7, 2),
(19, 'Tanpa Serial Number (Bulk)', 'SKU-FUR-0001', 'Kursi Kerja Ergonomis Staff', 'Furniture Kantor', 'Chairman', 'Unit', 'Asset', 1250000.00, 1250000.00, 'Kursi kantor hidrolik sandaran jaring hitam.', 15, 4, 11, 2),
(20, 'Tanpa Serial Number (Bulk)', 'SKU-FUR-0002', 'Meja Kantor L-Shape 120cm', 'Furniture Kantor', 'Indachi', 'Unit', 'Asset', 2100000.00, 2100000.00, 'Meja biro / staff kayu partikel lapis HPL.', 10, 4, 11, 2),
(21, 'Tanpa Serial Number (Bulk)', 'SKU-ELC-0001', 'AC Split Daikin 1 PK Standard', 'Elektronik Ruangan', 'Daikin', 'Unit', 'Asset', 4200000.00, 4200000.00, 'Air Conditioner Daikin 1 PK R32.', 6, 5, 13, 2),
(22, 'Tanpa Serial Number (Bulk)', 'SKU-ELC-0002', 'Kipas Angin Dinding 18 Inch', 'Elektronik Ruangan', 'Miyako', 'Unit', 'Asset', 350000.00, 350000.00, 'Wall fan 18 inch industrial grade.', 12, 5, 13, 2),
(23, 'Tanpa Serial Number (Bulk)', 'SKU-CLN-0001', 'Cairan Pembersih Lantai 5 Liter', 'Alat Kebersihan', 'Yuri', 'Gallon', 'Consumable (Habis Pakai)', 75000.00, 75000.00, 'Floor cleaner aroma pine antiseptic.', 20, 6, 15, 21),
(24, 'Tanpa Serial Number (Bulk)', 'SKU-CLN-0002', 'Tissue Roll Toilet Basah/Kering', 'Alat Kebersihan', 'Nice', 'Pack', 'Consumable (Habis Pakai)', 18000.00, 18000.00, 'Tissue roll isi 10 roll per pack.', 35, 6, 15, 7),
(25, 'Pelacakan Serial Number (SN)', 'SKU-SEC-0001', 'IP Camera CCTV Outdoor 2MP', 'Perlengkapan Keamanan', 'Hikvision', 'Unit', 'Asset', 850000.00, 850000.00, 'Hikvision Outdoor Infrared Network Camera.', 15, 7, 16, 2),
(26, 'Pelacakan Serial Number (SN)', 'SKU-SEC-0002', 'Digital Smart Door Lock Biometric', 'Perlengkapan Keamanan', 'Yale', 'Unit', 'Asset', 2800000.00, 2800000.00, 'Kunci pintu digital akses sidik jari dan RFID.', 5, 7, 16, 2),
(27, 'Pelacakan Serial Number (SN)', 'SKU-TST-0001', 'Digital Multimeter Fluke 101', 'Alat Ukur & Testing', 'Fluke', 'Unit', 'Asset', 950000.00, 950000.00, 'Pocket multimeter digital ringkas dan akurat.', 8, 8, 17, 2),
(28, 'Pelacakan Serial Number (SN)', 'SKU-AV-0001', 'Infocus Projector 3800 Lumens', 'Perangkat Audio Visual', 'Epson', 'Unit', 'Asset', 5800000.00, 5800000.00, 'Proyektor meeting room XGA resolution.', 4, 9, 18, 2),
(29, 'Tanpa Serial Number (Bulk)', 'SKU-AV-0002', 'Wireless Presentation Screen 84 Inch', 'Perangkat Audio Visual', 'Datalite', 'Unit', 'Asset', 1750000.00, 1750000.00, 'Layar proyektor gantung otomatis pakai remote.', 3, 9, 18, 2),
(30, 'Pelacakan Serial Number (SN)', 'SKU-VEH-0001', 'Motor Operasional Honda Beat CBS', 'Kendaraan Operasional', 'Honda', 'Unit', 'Asset', 18500000.00, 18500000.00, 'Sepeda motor dinas lapangan matic 110cc.', 3, 10, 19, 2),
(31, 'Pelacakan Serial Number (SN)', 'SKU-COM-0001', 'IP Phone Grandstream GXP1625', 'Alat Komunikasi', 'Grandstream', 'Unit', 'Asset', 950000.00, 950000.00, 'IP Telephone 2-line SIP with PoE.', 12, 11, 20, 2),
(32, 'Tanpa Serial Number (Bulk)', 'SKU-MED-0001', 'Kotak P3K Dinding Besi Lengkap', 'Perlengkapan P3K', 'OneMed', 'Box', 'Asset', 175000.00, 175000.00, 'First aid box standar isi obat luar dan perban.', 10, 12, 21, 4),
(33, 'Pelacakan Serial Number (SN)', 'SKU-HWD-0001', 'Printer Barcode Thermal TSC TE244', 'Perangkat Hardware Pendukung', 'TSC', 'Unit', 'Asset', 2400000.00, 2400000.00, 'Printer label barcode cetak resi dan stiker.', 6, 13, 22, 2),
(34, 'Pelacakan Serial Number (SN)', 'SKU-HWD-0002', 'Barcode Scanner Wireless Omni', 'Perangkat Hardware Pendukung', 'Honeywell', 'Unit', 'Asset', 1450000.00, 1450000.00, 'Laser barcode scanner 1D/2D wireless bluetooth.', 8, 13, 22, 2),
(35, 'Tanpa Serial Number (Bulk)', 'SKU-SFT-0001', 'Lisensi Microsoft Office Home & Business', 'Software & Lisensi', 'Microsoft', 'Pcs', 'Asset', 2800000.00, 2800000.00, 'Product key digital license lifetime for 1 PC.', 20, 14, 23, 1),
(36, 'Tanpa Serial Number (Bulk)', 'SKU-SFT-0002', 'Antivirus Kaspersky Endpoint Security', 'Software & Lisensi', 'Kaspersky', 'Pcs', 'Asset', 350000.00, 350000.00, 'Subscription 1 year corporate workstation.', 25, 14, 23, 1),
(37, 'Tanpa Serial Number (Bulk)', 'SKU-MER-0001', 'Tumbler Stainless Steel Custom Logo', 'Merchandise & Souvenir', 'Corkcicle', 'Pcs', 'Consumable (Habis Pakai)', 120000.00, 120000.00, 'Botol minum vakum tahan panas dan dingin.', 50, 15, 24, 1),
(38, 'Tanpa Serial Number (Bulk)', 'SKU-MER-0002', 'Kaos Polo Shirt Uniform Perusahaan', 'Merchandise & Souvenir', 'Lacoste', 'Pcs', 'Consumable (Habis Pakai)', 95000.00, 95000.00, 'Kaos kerah seragam bordir logo perusahaan.', 40, 15, 24, 1),
(39, 'Pelacakan Serial Number (SN)', 'SKU-LAB-0001', 'Digital PH Meter Benchtop', 'Alat Laboratorium', 'Ohaus', 'Unit', 'Asset', 3200000.00, 3200000.00, 'Alat ukur tingkat keasaman cairan lab presisi tinggi.', 2, 16, 25, 2),
(40, 'Tanpa Serial Number (Bulk)', 'SKU-LOG-0001', 'Plastik Bubble Wrap 125 Meter', 'Material Logistik', 'Generic', 'Roll', 'Consumable (Habis Pakai)', 175000.00, 175000.00, 'Plastik gelembung pengaman packing gulungan besar.', 15, 17, 26, 8),
(41, 'Tanpa Serial Number (Bulk)', 'SKU-LOG-0002', 'Kardus Box Polos Ukuran Sedang', 'Material Logistik', 'Generic', 'Pcs', 'Consumable (Habis Pakai)', 6500.00, 6500.00, 'Kardus arsip / packing tebal double wall.', 100, 17, 26, 1),
(42, 'Pelacakan Serial Number (SN)', 'SKU-ITE-0008', 'Monitor Dell 24 Inch IPS', 'IT Equipment', 'Dell', 'Unit', 'Asset', 2100000.00, 2100000.00, 'Monitor LED IPS Full HD HDMI DisplayPort.', 12, 18, 27, 2),
(43, 'Pelacakan Serial Number (SN)', 'SKU-ITE-0009', 'Keyboard & Mouse Wireless Logitech MK295', 'IT Equipment', 'Logitech', 'Set', 'Asset', 380000.00, 380000.00, 'Silent wireless keyboard mouse combo.', 20, 18, 27, 3),
(44, 'Tanpa Serial Number (Bulk)', 'SKU-CON-0004', 'Stapler Heavy Duty HD-12N/24', 'Consumables', 'Max', 'Pcs', 'Asset', 210000.00, 210000.00, 'Hefter besar jilid kertas tebal.', 10, 19, 28, 1),
(45, 'Tanpa Serial Number (Bulk)', 'SKU-OFF-0003', 'Běnar / Map Folder Arsip Gantung', 'Office Supplies', 'Bantex', 'Pcs', 'Consumable (Habis Pakai)', 15000.00, 15000.00, 'Map gantung filing cabinet lemari arsip.', 60, 20, 29, 1),
(46, 'Pelacakan Serial Number (SN)', 'SKU-SRV-0002', 'UPS APC Smart-UPS 1500VA', 'Server & Storage', 'APC', 'Unit', 'Asset', 4200000.00, 4200000.00, 'Uninterruptible Power Supply backup battery server.', 4, 21, 30, 2),
(47, 'Tanpa Serial Number (Bulk)', 'SKU-ELC-0003', 'Dispenser Galon Bawah Modena', 'Elektronik Ruangan', 'Modena', 'Unit', 'Asset', 1850000.00, 1850000.00, 'Water dispenser hot, normal, and cold.', 5, 22, 31, 2),
(48, 'Tanpa Serial Number (Bulk)', 'SKU-CLN-0003', 'Sapu Lantai Nilon & Pengki Set', 'Alat Kebersihan', 'Lion Star', 'Set', 'Consumable (Habis Pakai)', 45000.00, 45000.00, 'Set alat kebersihan sapu dan serokan sampah.', 15, 23, 32, 3),
(49, 'Pelacakan Serial Number (SN)', 'SKU-SEC-0003', 'Metal Detector Handheld Security', 'Perlengkapan Keamanan', 'Garrett', 'Unit', 'Asset', 1250000.00, 1250000.00, 'Detektor logam genggam pengaman pintu masuk.', 4, 24, 33, 2),
(50, 'Pelacakan Serial Number (SN)', 'SKU-AV-0003', 'Speaker Portable Conference Meeting', 'Perangkat Audio Visual', 'JBL', 'Unit', 'Asset', 2500000.00, 2500000.00, 'Wireless bluetooth conference speakerphone.', 6, 25, 34, 2),
(51, 'Tanpa Serial Number (Bulk)', 'SKU-CON-0005', 'Batu Baterai Alkaline AA Energizer', 'Consumables', 'Energizer', 'Pack', 'Consumable (Habis Pakai)', 25000.00, 25000.00, 'Baterai ukuran AA isi 4 pcs.', 30, 26, 35, 7),
(52, 'Tanpa Serial Number (Bulk)', 'SKU-CON-0006', 'Lakban Coklat 2 Inch Daimaru', 'Consumables', 'Daimaru', 'Roll', 'Consumable (Habis Pakai)', 12000.00, 12000.00, 'Packaging tape lebar 2 inch panjang 100 yard.', 80, 27, 36, 8),
(53, 'Tanpa Serial Number (Bulk)', 'SKU-OFF-0004', 'Gunting Kertas Besar Joyko', 'Office Supplies', 'Joyko', 'Pcs', 'Consumable (Habis Pakai)', 12000.00, 12000.00, 'Gunting stainless steel multifungsi.', 25, 28, 37, 1),
(54, 'Tanpa Serial Number (Bulk)', 'SKU-OFF-0005', 'Correction Tape / Tip-Ex Kertas Joyko', 'Office Supplies', 'Joyko', 'Pcs', 'Consumable (Habis Pakai)', 7500.00, 7500.00, 'Pita koreksi tulisan pulpen bersih.', 40, 29, 38, 1),
(55, 'Pelacakan Serial Number (SN)', 'SKU-ITE-0010', 'External Hard Disk 2TB USB 3.0', 'IT Equipment', 'Seagate', 'Unit', 'Asset', 1150000.00, 1150000.00, 'Portable external HDD backup data storage.', 10, 30, 39, 2),
(56, 'Pelacakan Serial Number (SN)', 'SKU-NET-0003', 'Wi-Fi 6 Access Point TP-Link EAP610', 'Networking Devices', 'TP-Link', 'Unit', 'Asset', 1450000.00, 1450000.00, 'Ceiling mount business wireless access point.', 8, 31, 40, 2),
(57, 'Tanpa Serial Number (Bulk)', 'SKU-CON-0007', 'Spidol Permanen Snowman Hitam', 'Consumables', 'Snowman', 'Pcs', 'Consumable (Habis Pakai)', 8500.00, 8500.00, 'Permanent marker board and box marker.', 50, 32, 41, 1),
(58, 'Tanpa Serial Number (Bulk)', 'SKU-CON-0008', 'Highlighter / Spidol Penyorot Warnawarni', 'Consumables', 'Stabilo', 'Set', 'Consumable (Habis Pakai)', 35000.00, 35000.00, 'Highlighter Boss Original 4 warna.', 20, 33, 42, 3),
(59, 'Pelacakan Serial Number (SN)', 'SKU-ITE-0011', 'Tablet Samsung Galaxy Tab A9', 'IT Equipment', 'Samsung', 'Unit', 'Asset', 2500000.00, 2500000.00, 'Tablet Android layar 8.7 inch Wi-Fi LTE.', 7, 34, 43, 2);

-- --------------------------------------------------------

--
-- Table structure for table `produk_item`
--

CREATE TABLE `produk_item` (
  `Id` int(11) NOT NULL,
  `ProdukId` int(11) NOT NULL,
  `GudangId` int(11) NOT NULL,
  `RakId` int(11) NOT NULL,
  `SerialNumber` varchar(100) NOT NULL,
  `Status` varchar(30) NOT NULL,
  `QrCodePath` varchar(255) DEFAULT NULL,
  `CreatedAt` datetime(6) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `produk_item`
--

INSERT INTO `produk_item` (`Id`, `ProdukId`, `GudangId`, `RakId`, `SerialNumber`, `Status`, `QrCodePath`, `CreatedAt`) VALUES
(1, 1, 1, 1, 'ITE-0001-GDG-JKT-1-0001', 'Assigned', '/qrcodes/ITE-0001-GDG-JKT-1-0001.png', '2026-10-30 15:56:47.660465'),
(2, 1, 1, 1, 'ITE-0001-GDG-JKT-1-0002', 'Sold', '/qrcodes/ITE-0001-GDG-JKT-1-0002.png', '2026-10-30 15:56:47.696004'),
(4, 2, 1, 1, 'ITE-0002-GDG-JKT-1-0002', 'Dipinjam', '/qrcodes/ITE-0002-GDG-JKT-1-0002.png', '2026-10-31 09:59:30.077557'),
(5, 2, 1, 1, 'ITE-0002-GDG-JKT-1-0003', 'Lost', '/qrcodes/ITE-0002-GDG-JKT-1-0003.png', '2026-10-31 09:59:30.082883'),
(6, 5, 2, 1, 'ITE-0002-GDG-JKT-1-0004', 'In Stock', '/qrcodes/ITE-0002-GDG-JKT-1-0004.png', '2026-10-31 09:59:30.087245'),
(7, 2, 1, 1, 'ITE-0002-GDG-JKT-1-0005', 'In Stock', '/qrcodes/ITE-0002-GDG-JKT-1-0005.png', '2026-10-31 16:30:17.350883'),
(8, 1, 1, 1, 'ITE-0001-GDG-JKT-1-0003', 'Disposed', '/qrcodes/ITE-0001-GDG-JKT-1-0003.png', '2026-10-31 16:30:17.443454'),
(9, 6, 2, 2, 'ITE-0003-GDG-SBY-01-0001', 'In Stock', '/qrcodes/ITE-0003-GDG-SBY-01-0001.png', '2026-11-06 09:37:06.992220'),
(10, 6, 2, 2, 'ITE-0003-GDG-SBY-01-0002', 'In Stock', '/qrcodes/ITE-0003-GDG-SBY-01-0002.png', '2026-11-06 09:37:07.105849'),
(11, 6, 2, 2, 'ITE-0003-GDG-SBY-01-0003', 'In Stock', '/qrcodes/ITE-0003-GDG-SBY-01-0003.png', '2026-11-06 09:37:07.109891'),
(12, 6, 2, 2, 'ITE-0003-GDG-SBY-01-0004', 'In Stock', '/qrcodes/ITE-0003-GDG-SBY-01-0004.png', '2026-11-06 09:37:07.114005'),
(13, 6, 2, 2, 'ITE-0003-GDG-SBY-01-0005', 'In Stock', '/qrcodes/ITE-0003-GDG-SBY-01-0005.png', '2026-11-06 09:37:07.123693'),
(14, 6, 2, 2, 'ITE-0003-GDG-SBY-01-0006', 'In Stock', '/qrcodes/ITE-0003-GDG-SBY-01-0006.png', '2026-11-06 09:37:07.211624'),
(15, 6, 2, 2, 'ITE-0003-GDG-SBY-01-0007', 'In Stock', '/qrcodes/ITE-0003-GDG-SBY-01-0007.png', '2026-11-06 09:37:07.222634'),
(16, 6, 2, 2, 'ITE-0003-GDG-SBY-01-0008', 'In Stock', '/qrcodes/ITE-0003-GDG-SBY-01-0008.png', '2026-11-06 09:37:07.230624'),
(17, 6, 2, 2, 'ITE-0003-GDG-SBY-01-0009', 'In Stock', '/qrcodes/ITE-0003-GDG-SBY-01-0009.png', '2026-11-06 09:37:07.234610'),
(18, 6, 2, 2, 'ITE-0003-GDG-SBY-01-0010', 'In Stock', '/qrcodes/ITE-0003-GDG-SBY-01-0010.png', '2026-11-06 09:37:07.240145'),
(19, 6, 2, 2, 'ITE-0003-GDG-SBY-01-0011', 'In Stock', '/qrcodes/ITE-0003-GDG-SBY-01-0011.png', '2026-11-06 09:37:07.256538'),
(20, 6, 2, 2, 'ITE-0003-GDG-SBY-01-0012', 'In Stock', '/qrcodes/ITE-0003-GDG-SBY-01-0012.png', '2026-11-06 09:37:07.268762'),
(21, 6, 2, 2, 'ITE-0003-GDG-SBY-01-0013', 'In Stock', '/qrcodes/ITE-0003-GDG-SBY-01-0013.png', '2026-11-06 09:37:07.277402'),
(22, 6, 2, 2, 'ITE-0003-GDG-SBY-01-0014', 'In Stock', '/qrcodes/ITE-0003-GDG-SBY-01-0014.png', '2026-11-06 09:37:07.281894'),
(23, 6, 2, 2, 'ITE-0003-GDG-SBY-01-0015', 'In Stock', '/qrcodes/ITE-0003-GDG-SBY-01-0015.png', '2026-11-06 09:37:07.316620'),
(24, 6, 2, 2, 'ITE-0003-GDG-SBY-01-0016', 'In Stock', '/qrcodes/ITE-0003-GDG-SBY-01-0016.png', '2026-11-06 09:37:07.328426'),
(25, 6, 2, 2, 'ITE-0003-GDG-SBY-01-0017', 'In Stock', '/qrcodes/ITE-0003-GDG-SBY-01-0017.png', '2026-11-06 09:37:07.332287'),
(26, 6, 2, 2, 'ITE-0003-GDG-SBY-01-0018', 'In Stock', '/qrcodes/ITE-0003-GDG-SBY-01-0018.png', '2026-11-06 09:37:07.351496'),
(27, 6, 2, 2, 'ITE-0003-GDG-SBY-01-0019', 'In Stock', '/qrcodes/ITE-0003-GDG-SBY-01-0019.png', '2026-11-06 09:37:07.358459'),
(28, 6, 2, 2, 'ITE-0003-GDG-SBY-01-0020', 'In Stock', '/qrcodes/ITE-0003-GDG-SBY-01-0020.png', '2026-11-06 09:37:07.369306');

-- --------------------------------------------------------

--
-- Table structure for table `produk_settings`
--

CREATE TABLE `produk_settings` (
  `Id` int(11) NOT NULL,
  `produk_id` int(11) NOT NULL,
  `gudang_id` int(11) NOT NULL,
  `safety_stock` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `produk_settings`
--

INSERT INTO `produk_settings` (`Id`, `produk_id`, `gudang_id`, `safety_stock`) VALUES
(1, 3, 1, 20),
(2, 5, 2, 15),
(3, 6, 2, 25),
(4, 7, 3, 20),
(5, 9, 1, 15),
(6, 10, 1, 10),
(7, 11, 1, 12),
(8, 12, 1, 30),
(9, 13, 1, 40),
(10, 14, 1, 25),
(11, 15, 1, 20),
(12, 16, 1, 8),
(13, 17, 1, 10),
(14, 18, 1, 5),
(15, 19, 4, 15),
(16, 20, 4, 10),
(17, 21, 5, 8),
(18, 22, 5, 10),
(19, 23, 6, 20),
(20, 24, 6, 15),
(21, 25, 7, 12),
(22, 26, 7, 5),
(23, 27, 8, 8),
(24, 28, 9, 6),
(25, 29, 9, 5),
(26, 30, 10, 4),
(27, 31, 11, 10),
(28, 32, 12, 8),
(29, 33, 13, 10),
(30, 34, 13, 12),
(31, 35, 14, 15),
(32, 36, 14, 20),
(33, 37, 15, 25),
(34, 38, 15, 20),
(35, 39, 16, 4),
(36, 40, 17, 20),
(37, 41, 17, 30),
(38, 42, 18, 10),
(39, 43, 18, 15),
(40, 44, 19, 8),
(41, 45, 20, 25),
(42, 46, 21, 6),
(43, 47, 22, 5),
(44, 48, 23, 10),
(45, 49, 24, 6),
(46, 50, 25, 8),
(47, 51, 26, 15),
(48, 52, 27, 25),
(49, 53, 28, 12),
(50, 54, 29, 15),
(51, 55, 30, 10);

-- --------------------------------------------------------

--
-- Table structure for table `rak`
--

CREATE TABLE `rak` (
  `Id` int(11) NOT NULL,
  `Area` varchar(20) NOT NULL,
  `NamaRak` varchar(50) NOT NULL,
  `BinNo` varchar(20) NOT NULL,
  `KodeLokasi` varchar(50) NOT NULL,
  `Deskripsi` varchar(255) DEFAULT NULL,
  `IsActive` tinyint(1) NOT NULL,
  `GudangId` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `rak`
--

INSERT INTO `rak` (`Id`, `Area`, `NamaRak`, `BinNo`, `KodeLokasi`, `Deskripsi`, `IsActive`, `GudangId`) VALUES
(1, 'Zone A (Electronics)', 'Rak A - Baris 1', 'Bin 01', 'A-01-01', 'Rak khusus penyimpanan laptop dan smartphone.', 1, 1),
(2, 'Zone A (Electronics)', 'Rak B - Baris 1', 'Bin 01', 'B-01-01', 'Kawasan Industri Rungkut Block B-3, Surabaya', 1, 2),
(3, 'C-01-01', 'Rak-01', '01', 'C-01-01', 'Rak khusus barang elektronik', 1, 4),
(4, 'Zone C (Accessories)', 'Rak A - Baris 3', 'Bin 01', 'A-03-01', 'Rak aksesori komputer dan kabel.', 1, 1),
(5, 'Zone D (Consumables)', 'Rak B - Baris 2', 'Bin 01', 'B-02-01', 'Rak penyimpanan kertas, tinta, dan alat tulis.', 1, 1),
(6, 'Zone E (Network)', 'Rak C - Baris 1', 'Bin 01', 'C-01-01', 'Rak perangkat switch, router, dan fiber optik.', 1, 1),
(7, 'Zone F (Server)', 'Rak C - Baris 2', 'Bin 02', 'C-02-01', 'Rak komponen server dan media penyimpanan data.', 1, 1),
(8, 'Zone A (Electronics)', 'Rak A - Baris 1', 'Bin 01', 'SBY-A-01-01', 'Rak khusus penyimpanan perangkat elektronik cabang Surabaya.', 1, 2),
(9, 'Zone B (Hardware)', 'Rak A - Baris 2', 'Bin 02', 'SBY-A-02-01', 'Rak suku cadang dan hardware cabang Surabaya.', 1, 2),
(10, 'Zone C (Accessories)', 'Rak B - Baris 1', 'Bin 01', 'SBY-B-01-01', 'Rak aksesori dan perlengkapan kantor cabang Surabaya.', 1, 2),
(11, 'Zone A (General)', 'Rak Utama - 01', 'Bin 01', 'BDG-01-01', 'Rak penyimpanan umum gudang Bandung.', 1, 4),
(12, 'Zone B (General)', 'Rak Utama - 02', 'Bin 01', 'BDG-02-01', 'Rak penyimpanan barang cepat putar gudang Bandung.', 1, 4),
(13, 'Zone A (General)', 'Rak Utama - 01', 'Bin 01', 'SMG-01-01', 'Rak penyimpanan umum gudang Semarang.', 1, 5),
(14, 'Zone B (General)', 'Rak Utama - 02', 'Bin 01', 'SMG-02-01', 'Rak penyimpanan barang komersial gudang Semarang.', 1, 5),
(15, 'Zone A (General)', 'Rak Utama - 01', 'Bin 01', 'MLG-01-01', 'Rak penyimpanan umum gudang Malang.', 1, 6),
(16, 'Zone A (General)', 'Rak Utama - 01', 'Bin 01', 'MDN-01-01', 'Rak penyimpanan umum gudang Medan.', 1, 7),
(17, 'Zone A (General)', 'Rak Utama - 01', 'Bin 01', 'PLB-01-01', 'Rak penyimpanan umum gudang Palembang.', 1, 8),
(18, 'Zone A (General)', 'Rak Utama - 01', 'Bin 01', 'PKU-01-01', 'Rak penyimpanan umum gudang Pekanbaru.', 1, 9),
(19, 'Zone A (General)', 'Rak Utama - 01', 'Bin 01', 'BTM-01-01', 'Rak penyimpanan umum gudang Batam.', 1, 10),
(20, 'Zone A (General)', 'Rak Utama - 01', 'Bin 01', 'DPS-01-01', 'Rak penyimpanan umum gudang Denpasar.', 1, 11),
(21, 'Zone A (General)', 'Rak Utama - 01', 'Bin 01', 'MKS-01-01', 'Rak penyimpanan umum gudang Makassar.', 1, 12),
(22, 'Zone A (General)', 'Rak Utama - 01', 'Bin 01', 'BDJ-01-01', 'Rak penyimpanan umum gudang Banjarmasin.', 1, 13),
(23, 'Zone A (General)', 'Rak Utama - 01', 'Bin 01', 'SMD-01-01', 'Rak penyimpanan umum gudang Samarinda.', 1, 14),
(24, 'Zone A (General)', 'Rak Utama - 01', 'Bin 01', 'PNK-01-01', 'Rak penyimpanan umum gudang Pontianak.', 1, 15),
(25, 'Zone A (General)', 'Rak Utama - 01', 'Bin 01', 'MDO-01-01', 'Rak penyimpanan umum gudang Manado.', 1, 16),
(26, 'Zone A (General)', 'Rak Utama - 01', 'Bin 01', 'YOG-01-01', 'Rak penyimpanan umum gudang Yogyakarta.', 1, 17),
(27, 'Zone A (General)', 'Rak Utama - 01', 'Bin 01', 'SOL-01-01', 'Rak penyimpanan umum gudang Solo.', 1, 18),
(28, 'Zone A (General)', 'Rak Utama - 01', 'Bin 01', 'LMP-01-01', 'Rak penyimpanan umum gudang Lampung.', 1, 19),
(29, 'Zone A (General)', 'Rak Utama - 01', 'Bin 01', 'PDG-01-01', 'Rak penyimpanan umum gudang Padang.', 1, 20),
(30, 'Zone A (General)', 'Rak Utama - 01', 'Bin 01', 'JBI-01-01', 'Rak penyimpanan umum gudang Jambi.', 1, 21),
(31, 'Zone A (General)', 'Rak Utama - 01', 'Bin 01', 'BKS-01-01', 'Rak penyimpanan umum gudang Bekasi Timur.', 1, 22),
(32, 'Zone A (General)', 'Rak Utama - 01', 'Bin 01', 'TGR-01-01', 'Rak penyimpanan umum gudang Tangerang.', 1, 23),
(33, 'Zone A (General)', 'Rak Utama - 01', 'Bin 01', 'BGR-01-01', 'Rak penyimpanan umum gudang Bogor Sentul.', 1, 24),
(34, 'Zone A (General)', 'Rak Utama - 01', 'Bin 01', 'CRB-01-01', 'Rak penyimpanan umum gudang Cirebon.', 1, 25),
(35, 'Zone A (General)', 'Rak Utama - 01', 'Bin 01', 'PWT-01-01', 'Rak penyimpanan umum gudang Purwokerto.', 1, 26),
(36, 'Zone A (General)', 'Rak Utama - 01', 'Bin 01', 'KDR-01-01', 'Rak penyimpanan umum gudang Kediri.', 1, 27),
(37, 'Zone A (General)', 'Rak Utama - 01', 'Bin 01', 'JMR-01-01', 'Rak penyimpanan umum gudang Jember.', 1, 28),
(38, 'Zone A (General)', 'Rak Utama - 01', 'Bin 01', 'BLI-02-01', 'Rak penyimpanan umum gudang Singaraja.', 1, 29),
(39, 'Zone A (General)', 'Rak Utama - 01', 'Bin 01', 'LOM-01-01', 'Rak penyimpanan umum gudang Lombok.', 1, 30),
(40, 'Zone A (General)', 'Rak Utama - 01', 'Bin 01', 'KUP-01-01', 'Rak penyimpanan umum gudang Kupang.', 1, 31),
(41, 'Zone A (General)', 'Rak Utama - 01', 'Bin 01', 'AMB-01-01', 'Rak penyimpanan umum gudang Ambon.', 1, 32),
(42, 'Zone A (General)', 'Rak Utama - 01', 'Bin 01', 'SRG-01-01', 'Rak penyimpanan umum gudang Sorong.', 1, 33),
(43, 'Zone A (General)', 'Rak Utama - 01', 'Bin 01', 'JAY-01-01', 'Rak penyimpanan umum gudang Jayapura.', 1, 34),
(44, 'Zone A (General)', 'Rak Utama - 01', 'Bin 01', 'TMK-01-01', 'Rak penyimpanan umum gudang Timika.', 1, 35),
(45, 'Zone Arsip', 'Rak Arsip - 01', 'Bin 01', 'ARS-01-01', 'Rak penyimpanan dokumen fisik dan berkas penting pusat.', 1, 38),
(46, 'Zone Arsip', 'Rak Arsip - 02', 'Bin 01', 'ARS-02-01', 'Rak penyimpanan dokumen keuangan dan pajak.', 1, 38),
(47, 'Zone Arsip', 'Rak Arsip Sby', 'Rak Sby 01', 'ARS-SBY-01', 'Rak arsip dokumen cabang Surabaya.', 1, 39),
(48, 'Zone Retur', 'Rak Retur - 01', 'Bin 01', 'RET-01-01', 'Rak penampungan sementara barang retur pelanggan.', 1, 40),
(49, 'Zone Retur', 'Rak Retur - 02', 'Bin 01', 'RET-02-01', 'Rak penampungan barang retur cabang Surabaya.', 1, 41),
(50, 'Zone Sparepart', 'Rak Mesin - 01', 'Bin 01', 'SPR-01-01', 'Rak suku cadang mesin berat dan industri.', 1, 42),
(51, 'Zone Finished', 'Rak Produk Jadi - 1', 'Bin 01', 'FIN-01-01', 'Rak penumpukan barang jadi siap kirim Jakarta.', 1, 50),
(52, 'Zone QA', 'Rak Karantina - 1', 'Bin 01', 'QA-01-01', 'Rak pemeriksaan kualitas dan uji kelayakan barang.', 1, 53);

-- --------------------------------------------------------

--
-- Table structure for table `reparasi_aset`
--

CREATE TABLE `reparasi_aset` (
  `Id` int(11) NOT NULL,
  `ProdukItemId` int(11) NOT NULL,
  `TanggalMasuk` datetime(6) NOT NULL,
  `TanggalKeluar` datetime(6) DEFAULT NULL,
  `Keluhan` varchar(500) NOT NULL,
  `Biaya` decimal(18,2) NOT NULL,
  `StatusReparasi` int(11) NOT NULL,
  `KondisiBarang` longtext NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `reparasi_aset`
--

INSERT INTO `reparasi_aset` (`Id`, `ProdukItemId`, `TanggalMasuk`, `TanggalKeluar`, `Keluhan`, `Biaya`, `StatusReparasi`, `KondisiBarang`) VALUES
(1, 6, '2026-10-31 00:00:00.000000', '2026-11-07 00:00:00.000000', 'Screen blank, tidak bisa hidup.', 450000.00, 0, 'Rusak'),
(2, 5, '2026-10-31 00:00:00.000000', '2026-11-07 00:00:00.000000', 'Ganti Battrey', 650000.00, 1, 'Rusak'),
(3, 1, '2026-11-01 00:00:00.000000', '2026-11-05 00:00:00.000000', 'Ganti keyboard laptop yang error / macet', 350000.00, 1, 'Baik'),
(4, 2, '2026-11-02 00:00:00.000000', '2026-11-08 00:00:00.000000', 'Upgrade RAM dan pembersihan kipas pendingin', 500000.00, 1, 'Baik'),
(5, 4, '2026-11-03 00:00:00.000000', NULL, 'Engsel layar patah dan casing retak', 750000.00, 0, 'Rusak'),
(6, 6, '2026-11-04 00:00:00.000000', '2026-11-10 00:00:00.000000', 'Instal ulang sistem operasi dan driver', 150000.00, 1, 'Baik'),
(7, 7, '2026-11-05 00:00:00.000000', NULL, 'Port USB tidak berfungsi / tidak terdeteksi', 300000.00, 0, 'Rusak Ringan'),
(8, 8, '2026-11-06 00:00:00.000000', '2026-11-12 00:00:00.000000', 'Penggantian kabel power adaptor', 120000.00, 1, 'Baik'),
(9, 9, '2026-11-07 00:00:00.000000', NULL, 'Layar bergaris dan kedip-kedip', 1200000.00, 0, 'Rusak'),
(10, 10, '2026-11-08 00:00:00.000000', '2026-11-14 00:00:00.000000', 'Pembersihan debu total internal komponen', 200000.00, 1, 'Baik'),
(11, 11, '2026-11-09 00:00:00.000000', NULL, 'Baterai cepat habis / drop', 600000.00, 0, 'Rusak Ringan'),
(12, 12, '2026-11-10 00:00:00.000000', '2026-11-15 00:00:00.000000', 'Perbaikan jack audio port headphone', 250000.00, 1, 'Baik'),
(13, 13, '2026-11-11 00:00:00.000000', NULL, 'Touchpad tidak responsif', 400000.00, 0, 'Rusak Ringan'),
(14, 14, '2026-11-12 00:00:00.000000', NULL, 'Blue screen saat booting Windows', 350000.00, 0, 'Rusak'),
(15, 15, '2026-11-13 00:00:00.000000', NULL, 'Kipas berisik / bising saat load tinggi', 250000.00, 0, 'Rusak Ringan'),
(16, 16, '2026-11-14 00:00:00.000000', NULL, 'Koneksi Wi-Fi sering putus sendiri', 300000.00, 0, 'Rusak Ringan'),
(17, 17, '2026-11-15 00:00:00.000000', NULL, 'Speaker internal sember / pecah', 220000.00, 0, 'Rusak Ringan'),
(18, 18, '2026-11-15 00:00:00.000000', NULL, 'Ganti SSD NVMe yang terbakar', 950000.00, 0, 'Rusak'),
(19, 19, '2026-11-15 00:00:00.000000', NULL, 'Kamera webcam buram / tidak terdeteksi', 280000.00, 0, 'Rusak Ringan'),
(20, 20, '2026-11-15 00:00:00.000000', NULL, 'Port HDMI tidak mengeluarkan display', 450000.00, 0, 'Rusak'),
(21, 21, '2026-11-15 00:00:00.000000', NULL, 'Motherboard konsleting akibat cairan', 1800000.00, 0, 'Rusak Berat'),
(22, 22, '2026-11-15 00:00:00.000000', NULL, 'Penggantian pasta prosesor (thermal paste)', 100000.00, 0, 'Baik'),
(23, 23, '2026-11-15 00:00:00.000000', NULL, 'Tombol power macet', 150000.00, 0, 'Rusak Ringan'),
(24, 24, '2026-11-15 00:00:00.000000', NULL, 'Slot MicroSD card reader rusak', 200000.00, 0, 'Rusak Ringan'),
(25, 25, '2026-11-15 00:00:00.000000', NULL, 'Ganti heatsink fan set', 550000.00, 0, 'Rusak'),
(26, 26, '2026-11-15 00:00:00.000000', NULL, 'Kabel fleksibel LCD putus', 350000.00, 0, 'Rusak'),
(27, 27, '2026-11-15 00:00:00.000000', NULL, 'Bluetooth internal tidak aktif', 200000.00, 0, 'Rusak Ringan'),
(28, 28, '2026-11-15 00:00:00.000000', NULL, 'Upgrade kapasitas SSD 1TB', 1100000.00, 0, 'Baik'),
(29, 29, '2026-11-15 00:00:00.000000', NULL, 'Perbaikan casing retak sudut kiri', 300000.00, 0, 'Rusak Ringan'),
(30, 1, '2026-11-15 00:00:00.000000', NULL, 'Kalibrasi baterai dan BIOS update', 75000.00, 0, 'Baik'),
(31, 2, '2026-11-15 00:00:00.000000', NULL, 'Penggantian karet kaki laptop', 50000.00, 0, 'Baik'),
(32, 4, '2026-11-15 00:00:00.000000', NULL, 'Pembersihan port LAN RJ45', 80000.00, 0, 'Baik'),
(33, 5, '2026-11-15 00:00:00.000000', NULL, 'Ganti panel layar LED pecah', 1400000.00, 0, 'Rusak Berat'),
(34, 6, '2026-11-15 00:00:00.000000', NULL, 'Pengecekan tegangan arus DC Jack', 150000.00, 0, 'Rusak Ringan'),
(35, 7, '2026-11-15 00:00:00.000000', NULL, 'Perbaikan error driver fingerprint', 120000.00, 0, 'Baik'),
(36, 8, '2026-11-15 00:00:00.000000', NULL, 'Penyelamatan data dari harddisk bad sector', 450000.00, 0, 'Rusak'),
(37, 9, '2026-11-15 00:00:00.000000', NULL, 'Penggantian IC Power motherboard', 850000.00, 0, 'Rusak Berat'),
(38, 10, '2026-11-15 00:00:00.000000', NULL, 'Instalasi software lisensi office', 100000.00, 0, 'Baik'),
(39, 11, '2026-11-15 00:00:00.000000', NULL, 'Perbaikan slot RAM patah pin', 400000.00, 0, 'Rusak'),
(40, 12, '2026-11-15 00:00:00.000000', NULL, 'Ganti bezel layar bagian atas', 250000.00, 0, 'Rusak Ringan'),
(41, 13, '2026-11-15 00:00:00.000000', NULL, 'Pengecekan suhu overheat CPU', 150000.00, 0, 'Rusak Ringan'),
(42, 14, '2026-11-15 00:00:00.000000', NULL, 'Penggantian modul Wi-Fi card', 280000.00, 0, 'Rusak Ringan'),
(43, 15, '2026-11-15 00:00:00.000000', NULL, 'Perbaikan jalur PCB keyboard', 200000.00, 0, 'Rusak Ringan'),
(44, 16, '2026-11-15 00:00:00.000000', NULL, 'Reset password BIOS yang terkunci', 100000.00, 0, 'Baik'),
(45, 17, '2026-11-15 00:00:00.000000', NULL, 'Penggantian tombol keyboard huruf A dan S', 120000.00, 0, 'Rusak Ringan'),
(46, 18, '2026-11-15 00:00:00.000000', NULL, 'Pembersihan sisa korosi oksidasi', 180000.00, 0, 'Rusak Ringan'),
(47, 19, '2026-11-15 00:00:00.000000', NULL, 'Perbaikan port Type-C Thunderbolt', 600000.00, 0, 'Rusak'),
(48, 20, '2026-11-15 00:00:00.000000', NULL, 'Pengujian kestabilan burn-in test', 100000.00, 0, 'Baik'),
(49, 21, '2026-11-15 00:00:00.000000', NULL, 'Penggantian sekring adaptor charger', 75000.00, 0, 'Rusak Ringan'),
(50, 22, '2026-11-15 00:00:00.000000', NULL, 'Pengecekan dead pixel pada layar', 90000.00, 0, 'Baik'),
(51, 23, '2026-11-15 00:00:00.000000', NULL, 'Perbaikan engsel kanan oblak', 175000.00, 0, 'Rusak Ringan'),
(52, 24, '2026-11-15 00:00:00.000000', NULL, 'Upgrade RAM 16GB DDR4', 650000.00, 0, 'Baik');

-- --------------------------------------------------------

--
-- Table structure for table `stockopname`
--

CREATE TABLE `stockopname` (
  `Id` int(11) NOT NULL,
  `NoReferensi` varchar(30) NOT NULL,
  `GudangId` int(11) NOT NULL,
  `Auditor` longtext NOT NULL,
  `TanggalMulai` datetime(6) NOT NULL,
  `TanggalSelesai` datetime(6) DEFAULT NULL,
  `Status` int(11) NOT NULL,
  `Catatan` longtext DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `stockopname`
--

INSERT INTO `stockopname` (`Id`, `NoReferensi`, `GudangId`, `Auditor`, `TanggalMulai`, `TanggalSelesai`, `Status`, `Catatan`) VALUES
(1, 'SO-202611-0001', 1, 'Super Admin', '2026-11-01 09:02:49.510864', '2026-11-01 09:16:13.540086', 1, ''),
(2, 'SO-202611-0002', 2, 'Warehouse Manager', '2026-11-02 09:00:00.000000', '2026-11-02 12:00:00.000000', 1, 'Audit rutin gudang cabang Surabaya.'),
(3, 'SO-202611-0003', 4, 'Budi Santoso', '2026-11-03 08:30:00.000000', '2026-11-03 11:30:00.000000', 1, 'Audit awal pembukaan gudang Bandung.'),
(4, 'SO-202611-0004', 5, 'Cindy Lestari', '2026-11-04 09:15:00.000000', '2026-11-04 14:00:00.000000', 1, 'Pemeriksaan fisik stok gudang Semarang.'),
(5, 'SO-202611-0005', 6, 'Super Admin', '2026-11-05 10:00:00.000000', '2026-11-05 15:00:00.000000', 1, 'Stock opname bulanan gudang Malang.'),
(6, 'SO-202611-0006', 7, 'Warehouse Manager', '2026-11-06 08:00:00.000000', '2026-11-06 13:00:00.000000', 1, 'Audit inventaris gudang Medan.'),
(7, 'SO-202611-0007', 8, 'Budi Santoso', '2026-11-07 09:00:00.000000', '2026-11-07 12:30:00.000000', 1, 'Pemeriksaan fisik gudang Palembang.'),
(8, 'SO-202611-0008', 9, 'Cindy Lestari', '2026-11-08 08:30:00.000000', '2026-11-08 14:30:00.000000', 1, 'Audit triwulan gudang Pekanbaru.'),
(9, 'SO-202611-0009', 10, 'Super Admin', '2026-11-09 10:00:00.000000', '2026-11-09 16:00:00.000000', 1, 'Stock opname gudang Batam.'),
(10, 'SO-202611-0010', 11, 'Warehouse Manager', '2026-11-10 09:00:00.000000', '2026-11-10 13:00:00.000000', 1, 'Audit rutin gudang Denpasar Bali.'),
(11, 'SO-202611-0011', 12, 'Budi Santoso', '2026-11-11 08:00:00.000000', '2026-11-11 12:00:00.000000', 1, 'Pemeriksaan fisik gudang Makassar.'),
(12, 'SO-202611-0012', 13, 'Cindy Lestari', '2026-11-12 09:30:00.000000', '2026-11-12 15:30:00.000000', 1, 'Audit gudang Banjarmasin.'),
(13, 'SO-202611-0013', 14, 'Super Admin', '2026-11-13 08:30:00.000000', '2026-11-13 14:00:00.000000', 1, 'Stock opname gudang Samarinda.'),
(14, 'SO-202611-0014', 15, 'Warehouse Manager', '2026-11-14 10:00:00.000000', '2026-11-14 15:00:00.000000', 1, 'Audit gudang Pontianak.'),
(15, 'SO-202611-0015', 16, 'Budi Santoso', '2026-11-15 09:00:00.000000', NULL, 2, 'Proses audit fisik gudang Manado sedang berjalan.'),
(16, 'SO-202611-0016', 17, 'Cindy Lestari', '2026-11-15 09:30:00.000000', NULL, 2, 'Proses audit fisik gudang Yogyakarta.'),
(17, 'SO-202611-0017', 18, 'Super Admin', '2026-11-15 10:00:00.000000', NULL, 2, 'Proses audit fisik gudang Solo.'),
(18, 'SO-202611-0018', 19, 'Warehouse Manager', '2026-11-15 10:30:00.000000', NULL, 2, 'Proses audit fisik gudang Lampung.'),
(19, 'SO-202611-0019', 20, 'Budi Santoso', '2026-11-15 11:00:00.000000', NULL, 2, 'Proses audit fisik gudang Padang.'),
(20, 'SO-202611-0020', 21, 'Cindy Lestari', '2026-11-15 11:30:00.000000', NULL, 2, 'Proses audit fisik gudang Jambi.'),
(21, 'SO-202611-0021', 22, 'Super Admin', '2026-11-15 13:00:00.000000', NULL, 2, 'Proses audit fisik gudang Bekasi Timur.'),
(22, 'SO-202611-0022', 23, 'Warehouse Manager', '2026-11-15 13:30:00.000000', NULL, 2, 'Proses audit fisik gudang Tangerang.'),
(23, 'SO-202611-0023', 24, 'Budi Santoso', '2026-11-15 14:00:00.000000', NULL, 2, 'Proses audit fisik gudang Bogor Sentul.'),
(24, 'SO-202611-0024', 25, 'Cindy Lestari', '2026-11-15 14:30:00.000000', NULL, 2, 'Proses audit fisik gudang Cirebon.'),
(25, 'SO-202611-0025', 26, 'Super Admin', '2026-11-15 15:00:00.000000', NULL, 2, 'Proses audit fisik gudang Purwokerto.'),
(26, 'SO-202611-0026', 27, 'Warehouse Manager', '2026-11-15 15:30:00.000000', NULL, 2, 'Proses audit fisik gudang Kediri.'),
(27, 'SO-202611-0027', 28, 'Budi Santoso', '2026-11-15 16:00:00.000000', NULL, 2, 'Proses audit fisik gudang Jember.'),
(28, 'SO-202611-0028', 29, 'Cindy Lestari', '2026-11-15 16:30:00.000000', NULL, 2, 'Proses audit fisik gudang Singaraja.'),
(29, 'SO-202611-0029', 30, 'Super Admin', '2026-11-15 17:00:00.000000', NULL, 2, 'Proses audit fisik gudang Lombok.'),
(30, 'SO-202611-0030', 31, 'Warehouse Manager', '2026-11-15 17:30:00.000000', NULL, 2, 'Proses audit fisik gudang Kupang.'),
(31, 'SO-202611-0031', 32, 'Budi Santoso', '2026-11-15 18:00:00.000000', NULL, 2, 'Proses audit fisik gudang Ambon.'),
(32, 'SO-202611-0032', 33, 'Cindy Lestari', '2026-11-15 18:30:00.000000', NULL, 2, 'Proses audit fisik gudang Sorong.'),
(33, 'SO-202611-0033', 34, 'Super Admin', '2026-11-15 19:00:00.000000', NULL, 2, 'Proses audit fisik gudang Jayapura.'),
(34, 'SO-202611-0034', 35, 'Warehouse Manager', '2026-11-15 19:30:00.000000', NULL, 2, 'Proses audit fisik gudang Timika.'),
(35, 'SO-202611-0035', 38, 'Budi Santoso', '2026-11-16 08:00:00.000000', NULL, 2, 'Audit dokumen arsip pusat Jakarta.'),
(36, 'SO-202611-0036', 39, 'Cindy Lestari', '2026-11-16 08:30:00.000000', NULL, 2, 'Audit dokumen arsip cabang Surabaya.'),
(37, 'SO-202611-0037', 40, 'Super Admin', '2026-11-16 09:00:00.000000', NULL, 2, 'Pemeriksaan fisik barang retur Jakarta.'),
(38, 'SO-202611-0038', 41, 'Warehouse Manager', '2026-11-16 09:30:00.000000', NULL, 2, 'Pemeriksaan fisik barang retur Surabaya.'),
(39, 'SO-202611-0039', 42, 'Budi Santoso', '2026-11-16 10:00:00.000000', NULL, 2, 'Audit suku cadang mesin utama.'),
(40, 'SO-202611-0040', 43, 'Cindy Lestari', '2026-11-16 10:30:00.000000', NULL, 2, 'Audit sparepart IT.'),
(41, 'SO-202611-0041', 46, 'Super Admin', '2026-11-16 11:00:00.000000', NULL, 2, 'Stock opname gudang musiman promo.'),
(42, 'SO-202611-0042', 47, 'Warehouse Manager', '2026-11-16 11:30:00.000000', NULL, 2, 'Stock opname gudang musiman Lebaran.'),
(43, 'SO-202611-0043', 48, 'Budi Santoso', '2026-11-16 13:00:00.000000', NULL, 2, 'Audit bahan baku utama Karawang.'),
(44, 'SO-202611-0044', 49, 'Cindy Lestari', '2026-11-16 13:30:00.000000', NULL, 2, 'Audit bahan pendukung Serang.'),
(45, 'SO-202611-0045', 50, 'Super Admin', '2026-11-16 14:00:00.000000', NULL, 2, 'Audit barang jadi Jakarta.'),
(46, 'SO-202611-0046', 51, 'Warehouse Manager', '2026-11-16 14:30:00.000000', NULL, 2, 'Audit barang jadi Surabaya.'),
(47, 'SO-202611-0047', 52, 'Budi Santoso', '2026-11-16 15:00:00.000000', NULL, 2, 'Audit logistik kantor pusat.'),
(48, 'SO-202611-0048', 53, 'Cindy Lestari', '2026-11-16 15:30:00.000000', NULL, 2, 'Audit karantina dan QC Bandung.'),
(49, 'SO-202611-0049', 1, 'Super Admin', '2026-11-17 08:00:00.000000', NULL, 2, 'Stock opname berkala gudang utama Jakarta.'),
(50, 'SO-202611-0050', 2, 'Warehouse Manager', '2026-11-17 08:30:00.000000', NULL, 2, 'Stock opname berkala gudang Surabaya.');

-- --------------------------------------------------------

--
-- Table structure for table `stockopnamedetail`
--

CREATE TABLE `stockopnamedetail` (
  `Id` int(11) NOT NULL,
  `StockOpnameId` int(11) NOT NULL,
  `ProdukId` int(11) NOT NULL,
  `ProdukItemId` int(11) DEFAULT NULL,
  `StokSistem` int(11) NOT NULL,
  `StokFisik` int(11) NOT NULL,
  `AlasanSelisih` longtext DEFAULT NULL,
  `Keterangan` longtext DEFAULT NULL,
  `Kondisi` longtext DEFAULT NULL,
  `SerialNumber` longtext DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `stockopnamedetail`
--

INSERT INTO `stockopnamedetail` (`Id`, `StockOpnameId`, `ProdukId`, `ProdukItemId`, `StokSistem`, `StokFisik`, `AlasanSelisih`, `Keterangan`, `Kondisi`, `SerialNumber`) VALUES
(1, 1, 1, 1, 2, 0, '', '', 'Hilang (Lost)', 'ITE-0001-GDG-JKT-1-0001'),
(2, 1, 1, 2, 2, 1, '', '', 'Ditemukan (OK)', 'ITE-0001-GDG-JKT-1-0002'),
(3, 1, 2, 4, 3, 1, '', '', 'Ditemukan (OK)', 'ITE-0002-GDG-JKT-1-0002'),
(4, 1, 2, 6, 3, 1, '', '', 'Ditemukan (OK)', 'ITE-0002-GDG-JKT-1-0004'),
(5, 1, 2, 7, 3, 1, '', '', 'Ditemukan (OK)', 'ITE-0002-GDG-JKT-1-0005'),
(6, 1, 3, NULL, 10, 10, '', '', 'Ditemukan (OK)', NULL),
(7, 2, 5, 6, 1, 1, '', '', 'Ditemukan (OK)', 'ITE-0002-GDG-JKT-1-0004'),
(8, 2, 6, 9, 20, 20, '', '', 'Ditemukan (OK)', 'ITE-0003-GDG-SBY-01-0001'),
(9, 3, 19, NULL, 15, 15, '', '', 'Ditemukan (OK)', NULL),
(10, 4, 21, NULL, 6, 6, '', '', 'Ditemukan (OK)', NULL),
(11, 5, 12, NULL, 25, 25, '', '', 'Ditemukan (OK)', NULL),
(12, 6, 25, NULL, 15, 15, '', '', 'Ditemukan (OK)', NULL),
(13, 7, 27, NULL, 8, 8, '', '', 'Ditemukan (OK)', NULL),
(14, 8, 28, NULL, 4, 4, '', '', 'Ditemukan (OK)', NULL),
(15, 9, 50, NULL, 3, 3, '', '', 'Ditemukan (OK)', NULL),
(16, 10, 40, NULL, 15, 15, '', '', 'Ditemukan (OK)', NULL),
(17, 11, 23, NULL, 20, 20, '', '', 'Ditemukan (OK)', NULL),
(18, 12, 16, NULL, 4, 4, '', '', 'Ditemukan (OK)', NULL),
(19, 13, 41, NULL, 100, 100, '', '', 'Ditemukan (OK)', NULL),
(20, 14, 32, NULL, 10, 10, '', '', 'Ditemukan (OK)', NULL),
(21, 15, 10, NULL, 5, 5, '', '', 'Ditemukan (OK)', NULL),
(22, 16, 11, NULL, 10, 10, '', '', 'Ditemukan (OK)', NULL),
(23, 17, 12, NULL, 25, 25, '', '', 'Ditemukan (OK)', NULL),
(24, 18, 13, NULL, 50, 50, '', '', 'Ditemukan (OK)', NULL),
(25, 19, 14, NULL, 30, 30, '', '', 'Ditemukan (OK)', NULL),
(26, 20, 15, NULL, 40, 40, '', '', 'Ditemukan (OK)', NULL),
(27, 21, 16, NULL, 4, 4, '', '', 'Ditemukan (OK)', NULL),
(28, 22, 17, NULL, 8, 8, '', '', 'Ditemukan (OK)', NULL),
(29, 23, 18, NULL, 2, 2, '', '', 'Ditemukan (OK)', NULL),
(30, 24, 19, NULL, 15, 15, '', '', 'Ditemukan (OK)', NULL),
(31, 25, 20, NULL, 10, 10, '', '', 'Ditemukan (OK)', NULL),
(32, 26, 21, NULL, 6, 6, '', '', 'Ditemukan (OK)', NULL),
(33, 27, 22, NULL, 12, 12, '', '', 'Ditemukan (OK)', NULL),
(34, 28, 23, NULL, 20, 20, '', '', 'Ditemukan (OK)', NULL),
(35, 29, 24, NULL, 35, 35, '', '', 'Ditemukan (OK)', NULL),
(36, 30, 25, NULL, 15, 15, '', '', 'Ditemukan (OK)', NULL),
(37, 31, 26, NULL, 5, 5, '', '', 'Ditemukan (OK)', NULL),
(38, 32, 27, NULL, 8, 8, '', '', 'Ditemukan (OK)', NULL),
(39, 33, 28, NULL, 4, 4, '', '', 'Ditemukan (OK)', NULL),
(40, 34, 29, NULL, 3, 3, '', '', 'Ditemukan (OK)', NULL),
(41, 35, 45, NULL, 60, 60, '', '', 'Ditemukan (OK)', NULL),
(42, 36, 45, NULL, 30, 30, '', '', 'Ditemukan (OK)', NULL),
(43, 37, 41, NULL, 20, 20, '', '', 'Ditemukan (OK)', NULL),
(44, 38, 41, NULL, 15, 15, '', '', 'Ditemukan (OK)', NULL),
(45, 39, 33, NULL, 6, 6, '', '', 'Ditemukan (OK)', NULL),
(46, 40, 34, NULL, 8, 8, '', '', 'Ditemukan (OK)', NULL),
(47, 41, 37, NULL, 50, 50, '', '', 'Ditemukan (OK)', NULL),
(48, 42, 38, NULL, 40, 40, '', '', 'Ditemukan (OK)', NULL),
(49, 43, 40, NULL, 15, 15, '', '', 'Ditemukan (OK)', NULL),
(50, 44, 41, NULL, 100, 100, '', '', 'Ditemukan (OK)', NULL),
(51, 45, 42, NULL, 12, 12, '', '', 'Ditemukan (OK)', NULL),
(52, 46, 43, NULL, 20, 20, '', '', 'Ditemukan (OK)', NULL),
(53, 47, 44, NULL, 10, 10, '', '', 'Ditemukan (OK)', NULL),
(54, 48, 39, NULL, 2, 2, '', '', 'Ditemukan (OK)', NULL),
(55, 49, 1, 1, 2, 2, '', '', 'Ditemukan (OK)', 'ITE-0001-GDG-JKT-1-0001'),
(56, 50, 6, 9, 20, 20, '', '', 'Ditemukan (OK)', 'ITE-0003-GDG-SBY-01-0001');

-- --------------------------------------------------------

--
-- Table structure for table `suppliers`
--

CREATE TABLE `suppliers` (
  `Id` int(11) NOT NULL,
  `KodeSupplier` varchar(50) NOT NULL,
  `NamaSupplier` varchar(150) NOT NULL,
  `Email` varchar(100) NOT NULL,
  `Telepon` varchar(30) NOT NULL,
  `NamaPic` varchar(100) NOT NULL,
  `WhatsappPic` varchar(30) NOT NULL,
  `Alamat` longtext NOT NULL,
  `Catatan` longtext NOT NULL,
  `IsAktif` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `suppliers`
--

INSERT INTO `suppliers` (`Id`, `KodeSupplier`, `NamaSupplier`, `Email`, `Telepon`, `NamaPic`, `WhatsappPic`, `Alamat`, `Catatan`, `IsAktif`) VALUES
(1, 'SPL-001', 'PT. Global Teknologi Indonesia', 'info@globaltech.co.id', '021-5558989', 'Andi Wijaya', '081234567890', 'Gedung Cyber Lt. 5, Jl. Kuningan Barat, Jakarta Selatan', 'Pemasok utama perangkat IT, server, dan jaringan.', 1),
(2, 'SPL-002', 'CV. Maju Bersama Logistik', 'sales@majubersama.com', '031-7772323', 'Budi Setiawan', '082199887766', 'Kawasan Industri SIER Blok F-12, Surabaya', 'Pemasok furniture kantor dan alat-alat gudang.', 1),
(3, 'SPL-003', 'PT. Sumber Makmur ATK', 'marketing@sumbermakmur.com', '021-3334545', 'Rina Rosiana', '085711223344', 'Jl. Mangga Dua Raya No. 45, Jakarta Utara', 'Pemasok kertas, tinta, whiteboard dan alat tulis kantor.', 1),
(4, 'SPL-004', 'PT. Nusantara Berkah Abadi', 'kontak@nusandraberkah.co.id', '022-4211234', 'Dewi Lestari', '081322334455', 'Jl. Asia Afrika No. 78, Bandung', 'Pemasok seragam dan perlengkapan keselamatan kerja (K3).', 1),
(5, 'SPL-005', 'CV. Jaya Mandiri Elektronik', 'jayamandiri@gmail.com', '024-8445678', 'Eko Prasetyo', '087833445566', 'Jl. Pemuda No. 120, Semarang', 'Pemasok peralatan elektronik dan pendingin ruangan (AC).', 1),
(6, 'SPL-006', 'PT. Solusi Digital Kreasi', 'halo@solusidigital.id', '021-9988776', 'Fajar Nugraha', '089644556677', 'Jl. Sudirman Kav. 52-53, Jakarta Pusat', 'Pemasok lisensi software dan perangkat lunak pendukung.', 1),
(7, 'SPL-007', 'CV. Berkah Tani Sejahtera', 'berkahtani@yahoo.com', '0341-367890', 'Siti Aminah', '085255667788', 'Jl. Borobudur No. 15, Malang', 'Pemasok kebutuhan pantry dan konsumsi kantor.', 1),
(8, 'SPL-008', 'PT. Mitra Teknik Perkasa', 'info@mitrateknik.co.id', '031-5342111', 'Hendra Gunawan', '081166778899', 'Jl. Rungkut Industri III No. 8, Surabaya', 'Pemasok suku cadang mesin dan alat teknik.', 1),
(9, 'SPL-009', 'CV. Karya Indah Furniture', 'karyaindah@gmail.com', '0274-512345', 'Maya Sari', '081977889900', 'Jl. Malioboro No. 45, Yogyakarta', 'Pemasok interior dan perabotan ruangan khusus.', 0),
(10, 'SPL-010', 'PT. Mega Buana Logistik', 'cs@megabuana.co.id', '061-4567890', 'Roni Kartono', '082388990011', 'Jl. Gatot Subroto No. 88, Medan', 'Pemasok layanan ekspedisi dan kemasan barang.', 1),
(11, 'SPL-011', 'CV. Citra Abadi Chemical', 'citraabadi@chemical.co.id', '021-8765432', 'Nina Marlina', '085699001122', 'Jl. Raya Industri Cikarang Blok B No. 3, Bekasi', 'Pemasok bahan pembersih dan cairan kimia kebersihan.', 1),
(12, 'SPL-012', 'PT. Delta Solusi Mandiri', 'support@deltasolusi.id', '031-8291029', 'Ahmad Fauzi', '087700112233', 'Jl. Jemursari No. 57, Surabaya', 'Pemasok perangkat jaringan dan kabel fiber optik.', 1),
(13, 'SPL-013', 'CV. Anugerah Jaya Packing', 'anugerahpacking@gmail.com', '022-6012345', 'Sri Wahyuni', '081211223344', 'Jl. Pasirkaliki No. 142, Bandung', 'Pemasok kardus, lakban, dan bahan packaging.', 1),
(14, 'SPL-014', 'PT. Bina Sarana Medika', 'binamedika@sarana.co.id', '021-7890123', 'Dr. Hendri', '081322334455', 'Jl. TB Simatupang No. 22, Jakarta Selatan', 'Pemasok kotak P3K dan perlengkapan medis darurat.', 1),
(15, 'SPL-015', 'CV. Tirta Segar Utama', 'tirtasegar@yahoo.com', '0251-8321456', 'Joko Susilo', '085733445566', 'Jl. Pajajaran No. 99, Bogor', 'Pemasok galon air mineral dan dispenser kantor.', 1),
(16, 'SPL-016', 'PT. Graha Multi Sarana', 'grahamulti@sarana.id', '031-5023456', 'Lestari Handayani', '089644556677', 'Jl. Gubeng Kertajaya No. 10, Surabaya', 'Pemasok alat kebersihan dan perlengkapan toilet.', 1),
(17, 'SPL-017', 'CV. Radja Promosi Mandiri', 'radjapromosi@gmail.com', '021-4205678', 'Dimas Anggara', '081955667788', 'Jl. Cempaka Putih Raya No. 14, Jakarta Pusat', 'Pemasok merchandise, plakat, dan souvenir perusahaan.', 0),
(18, 'SPL-018', 'PT. Universal Safetyindo', 'safety@universalindo.co.id', '021-8834567', 'Yuli Astuti', '082366778899', 'Jl. Jababeka Raya Blok C No. 5, Cikarang', 'Pemasok alat pelindung diri (APD) lengkap.', 1),
(19, 'SPL-019', 'CV. Cahaya Abadi Lampu', 'cahayaabadi@gmail.com', '022-7301234', 'Bambang Supriyadi', '087777889900', 'Jl. Buah Batu No. 180, Bandung', 'Pemasok lampu penerangan dan komponen elektrikal.', 1),
(20, 'SPL-020', 'PT. Paramount Solusi Data', 'contact@paramountdata.co.id', '021-5290123', 'Taufik Hidayat', '081288990011', 'Jl. Mega Kuningan Barat III, Jakarta Selatan', 'Pemasok perangkat server rack dan cooling system.', 1);

-- --------------------------------------------------------

--
-- Table structure for table `systemsettings`
--

CREATE TABLE `systemsettings` (
  `Id` int(11) NOT NULL,
  `SiteName` longtext NOT NULL,
  `SiteDescription` longtext NOT NULL,
  `CompanyAddress` longtext NOT NULL,
  `CompanyPhone` longtext NOT NULL,
  `CompanyEmail` longtext NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `systemsettings`
--

INSERT INTO `systemsettings` (`Id`, `SiteName`, `SiteDescription`, `CompanyAddress`, `CompanyPhone`, `CompanyEmail`) VALUES
(1, 'PT. CONTOH MAJU JAYA', 'Inventory Multi-Gudang Modern. Sinkronisasi stok antar-cabang, pantau valuasi aset, dan kelola mutasi barang presisi.', 'Komp. Perkantoran Indah Blok B-12, Jakarta Selatan, Indonesia', '(021) 555-9830', 'support@ptcontohmajujaya.com');

-- --------------------------------------------------------

--
-- Table structure for table `transaksistok`
--

CREATE TABLE `transaksistok` (
  `Id` int(11) NOT NULL,
  `NomorReferensi` longtext NOT NULL,
  `GudangId` int(11) NOT NULL,
  `SupplierId` int(11) DEFAULT NULL,
  `TipeTransaksi` longtext NOT NULL,
  `Catatan` longtext DEFAULT NULL,
  `Pembuat` longtext NOT NULL,
  `TanggalPosting` datetime(6) NOT NULL,
  `IsPenjualan` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `transaksistok`
--

INSERT INTO `transaksistok` (`Id`, `NomorReferensi`, `GudangId`, `SupplierId`, `TipeTransaksi`, `Catatan`, `Pembuat`, `TanggalPosting`, `IsPenjualan`) VALUES
(1, 'TRX-IN-20261031163017', 1, 1, 'Stock In', 'Penambahan Stock', 'Super Admin', '2026-10-31 16:30:17.104906', 0),
(2, 'TRX-OUT-20261031163206', 1, NULL, 'Stock Out', 'Penjuanlan barang', 'Super Admin', '2026-10-31 16:32:06.266880', 1),
(7, 'TRX-OUT-20261101092149', 1, NULL, 'Stock Out', NULL, 'System', '2026-11-01 09:21:49.623562', 1),
(8, 'TRX-IN-20261115-0001', 1, 1, 'Stock In', 'Pengadaan perangkat IT baru dari supplier utama', 'Super Admin', '2026-11-15 08:00:00.000000', 0),
(9, 'TRX-IN-20261115-0002', 1, 2, 'Stock In', 'Penambahan stok inventaris kantor dan furniture', 'Super Admin', '2026-11-15 08:30:00.000000', 0),
(10, 'TRX-OUT-20261115-0001', 1, NULL, 'Stock Out', 'Penjualan unit laptop komersial ke pelanggan', 'Super Admin', '2026-11-15 09:00:00.000000', 1),
(11, 'TRX-IN-20261115-0003', 2, 3, 'Stock In', 'Pengadaan alat tulis dan kertas cabang Surabaya', 'Warehouse Manager', '2026-11-15 09:30:00.000000', 0),
(12, 'TRX-IN-20261115-0004', 2, 5, 'Stock In', 'Penambahan perangkat elektronik ruangan pendingin', 'Warehouse Manager', '2026-11-15 10:00:00.000000', 0),
(13, 'TRX-OUT-20261115-0002', 2, NULL, 'Stock Out', 'Pengeluaran stok barang untuk operasional cabang', 'Warehouse Staff', '2026-11-15 10:30:00.000000', 1),
(14, 'TRX-IN-20261115-0005', 4, 1, 'Stock In', 'Pengiriman perangkat IT untuk gudang Bandung', 'Super Admin', '2026-11-15 11:00:00.000000', 0),
(15, 'TRX-IN-20261115-0006', 5, 2, 'Stock In', 'Pengadaan perlengkapan gudang Semarang', 'Super Admin', '2026-11-15 11:30:00.000000', 0),
(16, 'TRX-OUT-20261115-0003', 4, NULL, 'Stock Out', 'Distribusi alat kerja staff Bandung', 'System', '2026-11-15 12:00:00.000000', 1),
(17, 'TRX-IN-20261115-0007', 6, 3, 'Stock In', 'Penambahan stok ATK gudang Malang', 'Super Admin', '2026-11-15 12:30:00.000000', 0),
(18, 'TRX-IN-20261115-0008', 7, 4, 'Stock In', 'Pengadaan seragam dan perlengkapan K3 Medan', 'Super Admin', '2026-11-15 13:00:00.000000', 0),
(19, 'TRX-OUT-20261115-0004', 5, NULL, 'Stock Out', 'Pengeluaran inventaris kantor Semarang', 'System', '2026-11-15 13:30:00.000000', 1),
(20, 'TRX-IN-20261115-0009', 8, 5, 'Stock In', 'Pengadaan alat ukur teknik Palembang', 'Super Admin', '2026-11-15 14:00:00.000000', 0),
(21, 'TRX-IN-20261115-0010', 9, 6, 'Stock In', 'Pembelian lisensi software Pekanbaru', 'Super Admin', '2026-11-15 14:30:00.000000', 0),
(22, 'TRX-OUT-20261115-0005', 6, NULL, 'Stock Out', 'Distribusi konsumsi dan ATK Malang', 'System', '2026-11-15 15:00:00.000000', 1),
(23, 'TRX-IN-20261115-0011', 10, 8, 'Stock In', 'Pengadaan suku cadang Batam', 'Super Admin', '2026-11-15 15:30:00.000000', 0),
(24, 'TRX-IN-20261115-0012', 11, 10, 'Stock In', 'Pengadaan material logistik Denpasar', 'Super Admin', '2026-11-15 16:00:00.000000', 0),
(25, 'TRX-OUT-20261115-0006', 7, NULL, 'Stock Out', 'Penjualan perangkat keamanan Medan', 'System', '2026-11-15 16:30:00.000000', 1),
(26, 'TRX-IN-20261115-0013', 12, 11, 'Stock In', 'Pengadaan bahan kebersihan Makassar', 'Super Admin', '2026-11-15 17:00:00.000000', 0),
(27, 'TRX-IN-20261115-0014', 13, 12, 'Stock In', 'Pengadaan perangkat jaringan Banjarmasin', 'Super Admin', '2026-11-15 17:30:00.000000', 0),
(28, 'TRX-OUT-20261115-0007', 8, NULL, 'Stock Out', 'Pengeluaran alat ukur Palembang', 'System', '2026-11-15 18:00:00.000000', 1),
(29, 'TRX-IN-20261115-0015', 14, 13, 'Stock In', 'Pengadaan material packaging Samarinda', 'Super Admin', '2026-11-15 18:30:00.000000', 0),
(30, 'TRX-IN-20261115-0016', 15, 14, 'Stock In', 'Pengadaan perlengkapan medis Pontianak', 'Super Admin', '2026-11-15 19:00:00.000000', 0),
(31, 'TRX-OUT-20261115-0008', 9, NULL, 'Stock Out', 'Penjualan unit proyektor Pekanbaru', 'System', '2026-11-15 19:30:00.000000', 1),
(32, 'TRX-IN-20261115-0017', 16, 15, 'Stock In', 'Pengadaan galon air Manado', 'Super Admin', '2026-11-15 20:00:00.000000', 0),
(33, 'TRX-IN-20261115-0018', 17, 16, 'Stock In', 'Pengadaan alat kebersihan Yogyakarta', 'Super Admin', '2026-11-15 20:30:00.000000', 0),
(34, 'TRX-OUT-20261115-0009', 10, NULL, 'Stock Out', 'Pengeluaran sparepart Batam', 'System', '2026-11-15 21:00:00.000000', 1),
(35, 'TRX-IN-20261115-0019', 18, 18, 'Stock In', 'Pengadaan APD Solo', 'Super Admin', '2026-11-15 21:30:00.000000', 0),
(36, 'TRX-IN-20261115-0020', 19, 19, 'Stock In', 'Pengadaan komponen elektrikal Lampung', 'Super Admin', '2026-11-15 22:00:00.000000', 0),
(37, 'TRX-OUT-20261115-0010', 11, NULL, 'Stock Out', 'Distribusi logistik Denpasar', 'System', '2026-11-15 22:30:00.000000', 1),
(38, 'TRX-IN-20261115-0021', 20, 20, 'Stock In', 'Pengadaan server rack Padang', 'Super Admin', '2026-11-15 23:00:00.000000', 0),
(39, 'TRX-IN-20261115-0022', 21, 1, 'Stock In', 'Pengadaan perangkat IT Jambi', 'Super Admin', '2026-11-15 23:30:00.000000', 0),
(40, 'TRX-OUT-20261115-0011', 12, NULL, 'Stock Out', 'Pengeluaran bahan pembersih Makassar', 'System', '2026-11-16 00:00:00.000000', 1),
(41, 'TRX-IN-20261115-0023', 22, 2, 'Stock In', 'Pengadaan furniture Bekasi Timur', 'Super Admin', '2026-11-16 00:30:00.000000', 0),
(42, 'TRX-IN-20261115-0024', 23, 3, 'Stock In', 'Pengadaan ATK Tangerang', 'Super Admin', '2026-11-16 01:00:00.000000', 0),
(43, 'TRX-OUT-20261115-0012', 13, NULL, 'Stock Out', 'Pengeluaran perangkat jaringan Banjarmasin', 'System', '2026-11-16 01:30:00.000000', 1),
(44, 'TRX-IN-20261115-0025', 24, 5, 'Stock In', 'Pengadaan elektronik Bogor Sentul', 'Super Admin', '2026-11-16 02:00:00.000000', 0),
(45, 'TRX-IN-20261115-0026', 25, 6, 'Stock In', 'Pengadaan software Cirebon', 'Super Admin', '2026-11-16 02:30:00.000000', 0),
(46, 'TRX-OUT-20261115-0013', 14, NULL, 'Stock Out', 'Pengeluaran material packing Samarinda', 'System', '2026-11-16 03:00:00.000000', 1),
(47, 'TRX-IN-20261115-0027', 26, 8, 'Stock In', 'Pengadaan alat teknik Purwokerto', 'Super Admin', '2026-11-16 03:30:00.000000', 0),
(48, 'TRX-IN-20261115-0028', 27, 10, 'Stock In', 'Pengadaan logistik Kediri', 'Super Admin', '2026-11-16 04:00:00.000000', 0),
(49, 'TRX-OUT-20261115-0014', 15, NULL, 'Stock Out', 'Pengeluaran alat medis Pontianak', 'System', '2026-11-16 04:30:00.000000', 1),
(50, 'TRX-IN-20261115-0029', 28, 11, 'Stock In', 'Pengadaan bahan pembersih Jember', 'Super Admin', '2026-11-16 05:00:00.000000', 0),
(51, 'TRX-IN-20261115-0030', 29, 12, 'Stock In', 'Pengadaan jaringan Singaraja', 'Super Admin', '2026-11-16 05:30:00.000000', 0),
(52, 'TRX-OUT-20261115-0015', 16, NULL, 'Stock Out', 'Pengeluaran galon air Manado', 'System', '2026-11-16 06:00:00.000000', 1),
(53, 'TRX-IN-20261115-0031', 30, 13, 'Stock In', 'Pengadaan packaging Lombok', 'Super Admin', '2026-11-16 06:30:00.000000', 0),
(54, 'TRX-IN-20261115-0032', 31, 14, 'Stock In', 'Pengadaan P3K Kupang', 'Super Admin', '2026-11-16 07:00:00.000000', 0),
(55, 'TRX-OUT-20261115-0016', 17, NULL, 'Stock Out', 'Pengeluaran alat kebersihan Yogyakarta', 'System', '2026-11-16 07:30:00.000000', 1),
(56, 'TRX-IN-20261115-0033', 32, 15, 'Stock In', 'Pengadaan air mineral Ambon', 'Super Admin', '2026-11-16 08:00:00.000000', 0),
(57, 'TRX-IN-20261115-0034', 33, 16, 'Stock In', 'Pengadaan alat kebersihan Sorong', 'Super Admin', '2026-11-16 08:30:00.000000', 0);

-- --------------------------------------------------------

--
-- Table structure for table `transaksistokdetail`
--

CREATE TABLE `transaksistokdetail` (
  `Id` int(11) NOT NULL,
  `TransaksiStokId` int(11) NOT NULL,
  `ProdukId` int(11) NOT NULL,
  `RakId` int(11) NOT NULL,
  `Jumlah` int(11) NOT NULL,
  `SerialNumbers` longtext DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `transaksistokdetail`
--

INSERT INTO `transaksistokdetail` (`Id`, `TransaksiStokId`, `ProdukId`, `RakId`, `Jumlah`, `SerialNumbers`) VALUES
(1, 1, 2, 1, 1, NULL),
(2, 1, 1, 1, 1, NULL),
(3, 2, 2, 1, 1, 'ITE-0002-GDG-JKT-1-0001'),
(8, 7, 1, 1, 1, 'ITE-0001-GDG-JKT-1-0002');

-- --------------------------------------------------------

--
-- Table structure for table `transferorderdetails`
--

CREATE TABLE `transferorderdetails` (
  `Id` int(11) NOT NULL,
  `TransferOrderId` int(11) NOT NULL,
  `ProdukId` int(11) NOT NULL,
  `KuantitasPermintaan` int(11) NOT NULL,
  `KuantitasDiterima` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `transferorderdetails`
--

INSERT INTO `transferorderdetails` (`Id`, `TransferOrderId`, `ProdukId`, `KuantitasPermintaan`, `KuantitasDiterima`) VALUES
(1, 1, 2, 1, 1),
(2, 2, 2, 1, 1),
(3, 3, 10, 2, 2),
(4, 4, 19, 3, 3),
(5, 5, 21, 2, 2),
(6, 6, 12, 5, 5),
(7, 7, 25, 2, 2),
(8, 8, 27, 2, 2),
(9, 9, 28, 1, 1),
(10, 10, 33, 2, 2),
(11, 11, 40, 5, 5),
(12, 12, 23, 4, 4),
(13, 13, 16, 2, 2),
(14, 14, 41, 10, 10),
(15, 15, 32, 2, 2),
(16, 16, 23, 3, 3),
(17, 17, 48, 5, 5),
(18, 18, 38, 5, 5),
(19, 19, 21, 2, 2),
(20, 20, 18, 1, 1),
(21, 21, 11, 2, 2),
(22, 22, 19, 2, 2),
(23, 23, 12, 5, 5),
(24, 24, 22, 2, 2),
(25, 25, 35, 3, 3),
(26, 26, 27, 2, 2),
(27, 27, 40, 3, 3),
(28, 28, 24, 5, 5),
(29, 29, 16, 1, 1),
(30, 30, 41, 10, 10),
(31, 31, 32, 2, 2),
(32, 32, 23, 4, 4),
(33, 33, 48, 3, 3),
(34, 34, 10, 2, 2),
(35, 35, 11, 2, 2),
(36, 36, 45, 10, 10),
(37, 37, 45, 5, 5),
(38, 38, 41, 5, 5),
(39, 39, 41, 3, 3),
(40, 40, 33, 2, 2),
(41, 41, 34, 2, 2),
(42, 42, 37, 10, 10),
(43, 43, 38, 10, 10),
(44, 44, 40, 5, 5),
(45, 45, 41, 10, 10),
(46, 46, 50, 2, 2),
(47, 47, 50, 2, 2),
(48, 48, 14, 5, 5),
(49, 49, 39, 1, 1),
(50, 50, 1, 1, 1),
(51, 51, 6, 5, 5),
(52, 52, 6, 3, 3);

-- --------------------------------------------------------

--
-- Table structure for table `transferorders`
--

CREATE TABLE `transferorders` (
  `Id` int(11) NOT NULL,
  `NomorDokumen` varchar(50) NOT NULL,
  `GudangAsalId` int(11) NOT NULL,
  `GudangTujuanId` int(11) NOT NULL,
  `Catatan` longtext DEFAULT NULL,
  `Status` int(11) NOT NULL,
  `TanggalDibuat` datetime(6) NOT NULL,
  `TanggalDikirim` datetime(6) DEFAULT NULL,
  `TanggalDiterima` datetime(6) DEFAULT NULL,
  `Pembuat` longtext NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `transferorders`
--

INSERT INTO `transferorders` (`Id`, `NomorDokumen`, `GudangAsalId`, `GudangTujuanId`, `Catatan`, `Status`, `TanggalDibuat`, `TanggalDikirim`, `TanggalDiterima`, `Pembuat`) VALUES
(1, 'TO/20261101/0001', 1, 2, 'Pindah asset ke gudang surabaya.', 2, '2026-11-01 09:55:49.378928', NULL, '2026-11-01 09:55:56.489880', ''),
(2, 'TO/20261101/0002', 1, 2, '', 2, '2026-11-01 16:32:57.858538', NULL, '2026-11-01 16:34:13.549836', ''),
(3, 'TO/20261115/0001', 1, 2, 'Transfer perangkat IT dan laptop ke gudang Surabaya', 2, '2026-11-15 08:00:00.000000', '2026-11-15 08:30:00.000000', '2026-11-15 09:00:00.000000', 'Super Admin'),
(4, 'TO/20261115/0002', 1, 4, 'Pengiriman inventaris furniture ke gudang Bandung', 2, '2026-11-15 09:00:00.000000', '2026-11-15 09:30:00.000000', '2026-11-15 10:00:00.000000', 'Super Admin'),
(5, 'TO/20261115/0003', 1, 5, 'Distribusi perangkat AC ke gudang Semarang', 2, '2026-11-15 10:00:00.000000', '2026-11-15 10:30:00.000000', '2026-11-15 11:00:00.000000', 'Super Admin'),
(6, 'TO/20261115/0004', 1, 6, 'Pengiriman ATK dan kertas ke gudang Malang', 2, '2026-11-15 11:00:00.000000', '2026-11-15 11:30:00.000000', '2026-11-15 12:00:00.000000', 'Super Admin'),
(7, 'TO/20261115/0005', 1, 7, 'Pengiriman CCTV dan keamanan ke gudang Medan', 2, '2026-11-15 12:00:00.000000', '2026-11-15 12:30:00.000000', '2026-11-15 13:00:00.000000', 'Super Admin'),
(8, 'TO/20261115/0006', 1, 8, 'Distribusi alat ukur teknik ke gudang Palembang', 2, '2026-11-15 13:00:00.000000', '2026-11-15 13:30:00.000000', '2026-11-15 14:00:00.000000', 'Super Admin'),
(9, 'TO/20261115/0007', 1, 9, 'Pengiriman proyektor dan AV ke gudang Pekanbaru', 2, '2026-11-15 14:00:00.000000', '2026-11-15 14:30:00.000000', '2026-11-15 15:00:00.000000', 'Super Admin'),
(10, 'TO/20261115/0008', 1, 10, 'Transfer suku cadang ke gudang Batam', 2, '2026-11-15 15:00:00.000000', '2026-11-15 15:30:00.000000', '2026-11-15 16:00:00.000000', 'Super Admin'),
(11, 'TO/20261115/0009', 1, 11, 'Pengiriman material logistik ke gudang Denpasar', 2, '2026-11-15 16:00:00.000000', '2026-11-15 16:30:00.000000', '2026-11-15 17:00:00.000000', 'Super Admin'),
(12, 'TO/20261115/0010', 1, 12, 'Distribusi bahan kebersihan ke gudang Makassar', 2, '2026-11-15 17:00:00.000000', '2026-11-15 17:30:00.000000', '2026-11-15 18:00:00.000000', 'Super Admin'),
(13, 'TO/20261115/0011', 1, 13, 'Pengiriman perangkat jaringan ke gudang Banjarmasin', 2, '2026-11-15 18:00:00.000000', '2026-11-15 18:30:00.000000', '2026-11-15 19:00:00.000000', 'Super Admin'),
(14, 'TO/20261115/0012', 1, 14, 'Pengiriman material packaging ke gudang Samarinda', 2, '2026-11-15 19:00:00.000000', '2026-11-15 19:30:00.000000', '2026-11-15 20:00:00.000000', 'Super Admin'),
(15, 'TO/20261115/0013', 1, 15, 'Pengiriman P3K dan medis ke gudang Pontianak', 2, '2026-11-15 20:00:00.000000', '2026-11-15 20:30:00.000000', '2026-11-15 21:00:00.000000', 'Super Admin'),
(16, 'TO/20261115/0014', 1, 16, 'Distribusi galon air ke gudang Manado', 2, '2026-11-15 21:00:00.000000', '2026-11-15 21:30:00.000000', '2026-11-15 22:00:00.000000', 'Super Admin'),
(17, 'TO/20261115/0015', 1, 17, 'Pengiriman alat kebersihan ke gudang Yogyakarta', 2, '2026-11-15 22:00:00.000000', '2026-11-15 22:30:00.000000', '2026-11-15 23:00:00.000000', 'Super Admin'),
(18, 'TO/20261115/0016', 1, 18, 'Pengiriman APD ke gudang Solo', 2, '2026-11-15 23:00:00.000000', '2026-11-15 23:30:00.000000', '2026-11-16 00:00:00.000000', 'Super Admin'),
(19, 'TO/20261115/0017', 1, 19, 'Pengiriman komponen elektrikal ke gudang Lampung', 2, '2026-11-16 00:00:00.000000', '2026-11-16 00:30:00.000000', '2026-11-16 01:00:00.000000', 'Super Admin'),
(20, 'TO/20261115/0018', 1, 20, 'Pengiriman server rack ke gudang Padang', 2, '2026-11-16 01:00:00.000000', '2026-11-16 01:30:00.000000', '2026-11-16 02:00:00.000000', 'Super Admin'),
(21, 'TO/20261115/0019', 1, 21, 'Pengiriman perangkat IT ke gudang Jambi', 2, '2026-11-16 02:00:00.000000', '2026-11-16 02:30:00.000000', '2026-11-16 03:00:00.000000', 'Super Admin'),
(22, 'TO/20261115/0020', 1, 22, 'Pengiriman furniture ke gudang Bekasi Timur', 2, '2026-11-16 03:00:00.000000', '2026-11-16 03:30:00.000000', '2026-11-16 04:00:00.000000', 'Super Admin'),
(23, 'TO/20261115/0021', 1, 23, 'Pengiriman ATK ke gudang Tangerang', 2, '2026-11-16 04:00:00.000000', '2026-11-16 04:30:00.000000', '2026-11-16 05:00:00.000000', 'Super Admin'),
(24, 'TO/20261115/0022', 1, 24, 'Pengiriman elektronik ke gudang Bogor Sentul', 2, '2026-11-16 05:00:00.000000', '2026-11-16 05:30:00.000000', '2026-11-16 06:00:00.000000', 'Super Admin'),
(25, 'TO/20261115/0023', 1, 25, 'Pengiriman software ke gudang Cirebon', 2, '2026-11-16 06:00:00.000000', '2026-11-16 06:30:00.000000', '2026-11-16 07:00:00.000000', 'Super Admin'),
(26, 'TO/20261115/0024', 1, 26, 'Pengiriman alat teknik ke gudang Purwokerto', 2, '2026-11-16 07:00:00.000000', '2026-11-16 07:30:00.000000', '2026-11-16 08:00:00.000000', 'Super Admin'),
(27, 'TO/20261115/0025', 1, 27, 'Pengiriman logistik ke gudang Kediri', 2, '2026-11-16 08:00:00.000000', '2026-11-16 08:30:00.000000', '2026-11-16 09:00:00.000000', 'Super Admin'),
(28, 'TO/20261115/0026', 1, 28, 'Pengiriman pembersih ke gudang Jember', 2, '2026-11-16 09:00:00.000000', '2026-11-16 09:30:00.000000', '2026-11-16 10:00:00.000000', 'Super Admin'),
(29, 'TO/20261115/0027', 1, 29, 'Pengiriman jaringan ke gudang Singaraja', 2, '2026-11-16 10:00:00.000000', '2026-11-16 10:30:00.000000', '2026-11-16 11:00:00.000000', 'Super Admin'),
(30, 'TO/20261115/0028', 1, 30, 'Pengiriman packaging ke gudang Lombok', 2, '2026-11-16 11:00:00.000000', '2026-11-16 11:30:00.000000', '2026-11-16 12:00:00.000000', 'Super Admin'),
(31, 'TO/20261115/0029', 1, 31, 'Pengiriman P3K ke gudang Kupang', 2, '2026-11-16 12:00:00.000000', '2026-11-16 12:30:00.000000', '2026-11-16 13:00:00.000000', 'Super Admin'),
(32, 'TO/20261115/0030', 1, 32, 'Pengiriman air mineral ke gudang Ambon', 2, '2026-11-16 13:00:00.000000', '2026-11-16 13:30:00.000000', '2026-11-16 14:00:00.000000', 'Super Admin'),
(33, 'TO/20261115/0031', 1, 33, 'Pengiriman alat kebersihan ke gudang Sorong', 2, '2026-11-16 14:00:00.000000', '2026-11-16 14:30:00.000000', '2026-11-16 15:00:00.000000', 'Super Admin'),
(34, 'TO/20261115/0032', 1, 34, 'Pengiriman inventaris ke gudang Jayapura', 2, '2026-11-16 15:00:00.000000', '2026-11-16 15:30:00.000000', '2026-11-16 16:00:00.000000', 'Super Admin'),
(35, 'TO/20261115/0033', 1, 35, 'Pengiriman inventaris ke gudang Timika', 2, '2026-11-16 16:00:00.000000', '2026-11-16 16:30:00.000000', '2026-11-16 17:00:00.000000', 'Super Admin'),
(36, 'TO/20261115/0034', 1, 38, 'Transfer arsip dokumen ke gudang arsip pusat', 2, '2026-11-16 17:00:00.000000', '2026-11-16 17:30:00.000000', '2026-11-16 18:00:00.000000', 'Super Admin'),
(37, 'TO/20261115/0035', 1, 39, 'Transfer arsip dokumen ke gudang arsip Surabaya', 2, '2026-11-16 18:00:00.000000', '2026-11-16 18:30:00.000000', '2026-11-16 19:00:00.000000', 'Super Admin'),
(38, 'TO/20261115/0036', 1, 40, 'Pengiriman barang retur ke gudang retur Jakarta', 2, '2026-11-16 19:00:00.000000', '2026-11-16 19:30:00.000000', '2026-11-16 20:00:00.000000', 'Super Admin'),
(39, 'TO/20261115/0037', 1, 41, 'Pengiriman barang retur ke gudang retur Surabaya', 2, '2026-11-16 20:00:00.000000', '2026-11-16 20:30:00.000000', '2026-11-16 21:00:00.000000', 'Super Admin'),
(40, 'TO/20261115/0038', 1, 42, 'Pengiriman suku cadang mesin ke gudang sparepart', 2, '2026-11-16 21:00:00.000000', '2026-11-16 21:30:00.000000', '2026-11-16 22:00:00.000000', 'Super Admin'),
(41, 'TO/20261115/0039', 1, 43, 'Pengiriman sparepart IT ke gudang sparepart IT', 2, '2026-11-16 22:00:00.000000', '2026-11-16 22:30:00.000000', '2026-11-16 23:00:00.000000', 'Super Admin'),
(42, 'TO/20261115/0040', 1, 46, 'Pengiriman barang musiman ke gudang seasonal', 2, '2026-11-16 23:00:00.000000', '2026-11-16 23:30:00.000000', '2026-11-17 00:00:00.000000', 'Super Admin'),
(43, 'TO/20261115/0041', 1, 47, 'Pengiriman barang musiman Lebaran ke gudang seasonal', 2, '2026-11-17 00:00:00.000000', '2026-11-17 00:30:00.000000', '2026-11-17 01:00:00.000000', 'Super Admin'),
(44, 'TO/20261115/0042', 1, 48, 'Pengiriman bahan baku ke gudang bahan baku utama', 2, '2026-11-17 01:00:00.000000', '2026-11-17 01:30:00.000000', '2026-11-17 02:00:00.000000', 'Super Admin'),
(45, 'TO/20261115/0043', 1, 49, 'Pengiriman bahan baku ke gudang bahan pendukung', 2, '2026-11-17 02:00:00.000000', '2026-11-17 02:30:00.000000', '2026-11-17 03:00:00.000000', 'Super Admin'),
(46, 'TO/20261115/0044', 1, 50, 'Pengiriman barang jadi ke gudang finished goods Jkt', 2, '2026-11-17 03:00:00.000000', '2026-11-17 03:30:00.000000', '2026-11-17 04:00:00.000000', 'Super Admin'),
(47, 'TO/20261115/0045', 1, 51, 'Pengiriman barang jadi ke gudang finished goods Sby', 2, '2026-11-17 04:00:00.000000', '2026-11-17 04:30:00.000000', '2026-11-17 05:00:00.000000', 'Super Admin'),
(48, 'TO/20261115/0046', 1, 52, 'Pengiriman logistik ke gudang kantor pusat', 2, '2026-11-17 05:00:00.000000', '2026-11-17 05:30:00.000000', '2026-11-17 06:00:00.000000', 'Super Admin'),
(49, 'TO/20261115/0047', 1, 53, 'Pengiriman sampel uji ke gudang karantina QA', 2, '2026-11-17 06:00:00.000000', '2026-11-17 06:30:00.000000', '2026-11-17 07:00:00.000000', 'Super Admin'),
(50, 'TO/20261115/0048', 2, 1, 'Pengiriman balik inventaris ke gudang utama Jakarta', 2, '2026-11-17 07:00:00.000000', '2026-11-17 07:30:00.000000', '2026-11-17 08:00:00.000000', 'Warehouse Manager'),
(51, 'TO/20261115/0049', 2, 4, 'Transfer stok antar cabang Surabaya ke Bandung', 2, '2026-11-17 08:00:00.000000', '2026-11-17 08:30:00.000000', '2026-11-17 09:00:00.000000', 'Warehouse Manager'),
(52, 'TO/20261115/0050', 4, 2, 'Transfer stok antar cabang Bandung ke Surabaya', 2, '2026-11-17 09:00:00.000000', '2026-11-17 09:30:00.000000', '2026-11-17 10:00:00.000000', 'Warehouse Manager');

-- --------------------------------------------------------

--
-- Table structure for table `__efmigrationshistory`
--

CREATE TABLE `__efmigrationshistory` (
  `MigrationId` varchar(150) NOT NULL,
  `ProductVersion` varchar(32) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `__efmigrationshistory`
--

INSERT INTO `__efmigrationshistory` (`MigrationId`, `ProductVersion`) VALUES
('20261030084151_InitCreate', '9.0.14'),
('20261103065023_UpdateApplicationUser', '9.0.14'),
('20261106033313_AllowNullIdRak', '9.0.14'),
('20261107013148_AddFKTblUOM', '9.0.14'),
('20261107021329_TambahMasterUomIdKeProduk', '9.0.14'),
('20261108011412_AddNewFielUOM', '9.0.14'),
('20261108012935_AddMasterUomId', '9.0.14'),
('20261108031157_AddMasterUomIdToProduk', '9.0.14'),
('20261110034408_Add3fieldtblKaryawan', '9.0.14'),
('20261110084922_EdittblKarywn', '9.0.14'),
('20261111010442_AddnewrelasiApplicationUser', '9.0.14');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `activitylogs`
--
ALTER TABLE `activitylogs`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_activitylogs_timestamp` (`Timestamp`),
  ADD KEY `IX_activitylogs_subject` (`Subject`);

--
-- Indexes for table `aspnetroleclaims`
--
ALTER TABLE `aspnetroleclaims`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_AspNetRoleClaims_RoleId` (`RoleId`);

--
-- Indexes for table `aspnetroles`
--
ALTER TABLE `aspnetroles`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `RoleNameIndex` (`NormalizedName`);

--
-- Indexes for table `aspnetuserclaims`
--
ALTER TABLE `aspnetuserclaims`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_AspNetUserClaims_UserId` (`UserId`);

--
-- Indexes for table `aspnetuserlogins`
--
ALTER TABLE `aspnetuserlogins`
  ADD PRIMARY KEY (`LoginProvider`,`ProviderKey`),
  ADD KEY `IX_AspNetUserLogins_UserId` (`UserId`);

--
-- Indexes for table `aspnetuserroles`
--
ALTER TABLE `aspnetuserroles`
  ADD PRIMARY KEY (`UserId`,`RoleId`),
  ADD KEY `IX_AspNetUserRoles_RoleId` (`RoleId`);

--
-- Indexes for table `aspnetusers`
--
ALTER TABLE `aspnetusers`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `UserNameIndex` (`NormalizedUserName`),
  ADD KEY `EmailIndex` (`NormalizedEmail`),
  ADD KEY `IX_AspNetUsers_KaryawanId` (`KaryawanId`);

--
-- Indexes for table `aspnetusertokens`
--
ALTER TABLE `aspnetusertokens`
  ADD PRIMARY KEY (`UserId`,`LoginProvider`,`Name`);

--
-- Indexes for table `customers`
--
ALTER TABLE `customers`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_customers_kodecustomer` (`KodeCustomer`);

--
-- Indexes for table `departments`
--
ALTER TABLE `departments`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `gudang`
--
ALTER TABLE `gudang`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `karyawans`
--
ALTER TABLE `karyawans`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_Karyawans_DepartmentId` (`DepartmentId`);

--
-- Indexes for table `kategoris`
--
ALTER TABLE `kategoris`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `master_uom`
--
ALTER TABLE `master_uom`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `IX_master_uom_Code` (`Code`);

--
-- Indexes for table `menus`
--
ALTER TABLE `menus`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `peminjamanaset`
--
ALTER TABLE `peminjamanaset`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_peminjamanaset_KaryawanId` (`KaryawanId`),
  ADD KEY `IX_peminjamanaset_kodepeminjaman` (`KodePeminjaman`),
  ADD KEY `IX_peminjamanaset_status` (`Status`);

--
-- Indexes for table `peminjamanasetdetail`
--
ALTER TABLE `peminjamanasetdetail`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_peminjamanasetdetail_PeminjamanAsetID` (`PeminjamanAsetID`),
  ADD KEY `IX_peminjamanasetdetail_ProdukItemId` (`ProdukItemId`);

--
-- Indexes for table `penugasanasetdetail`
--
ALTER TABLE `penugasanasetdetail`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_penugasanasetdetail_PenugasanAsetId` (`PenugasanAsetId`);

--
-- Indexes for table `penugasanasets`
--
ALTER TABLE `penugasanasets`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_penugasanasets_kodepenugasan` (`KodePenugasan`),
  ADD KEY `IX_penugasanasets_status` (`Status`);

--
-- Indexes for table `permissions`
--
ALTER TABLE `permissions`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `produk`
--
ALTER TABLE `produk`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_Produk_GudangId` (`GudangId`),
  ADD KEY `IX_Produk_RakId` (`RakId`),
  ADD KEY `IX_Produk_MasterUomId` (`MasterUomId`),
  ADD KEY `IX_produk_sku` (`Sku`);

--
-- Indexes for table `produk_item`
--
ALTER TABLE `produk_item`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_produk_item_GudangId` (`GudangId`),
  ADD KEY `IX_produk_item_ProdukId` (`ProdukId`),
  ADD KEY `IX_produk_item_RakId` (`RakId`),
  ADD KEY `IX_produk_item_serialnumber` (`SerialNumber`),
  ADD KEY `IX_produk_item_status` (`Status`);

--
-- Indexes for table `produk_settings`
--
ALTER TABLE `produk_settings`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_produk_settings_gudang_id` (`gudang_id`),
  ADD KEY `IX_produk_settings_produk_id` (`produk_id`);

--
-- Indexes for table `rak`
--
ALTER TABLE `rak`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_Rak_GudangId` (`GudangId`);

--
-- Indexes for table `reparasi_aset`
--
ALTER TABLE `reparasi_aset`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `stockopname`
--
ALTER TABLE `stockopname`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_StockOpname_GudangId` (`GudangId`),
  ADD KEY `IX_stockopname_noreferensi` (`NoReferensi`);

--
-- Indexes for table `stockopnamedetail`
--
ALTER TABLE `stockopnamedetail`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_StockOpnameDetail_ProdukId` (`ProdukId`),
  ADD KEY `IX_StockOpnameDetail_ProdukItemId` (`ProdukItemId`),
  ADD KEY `IX_StockOpnameDetail_StockOpnameId` (`StockOpnameId`);

--
-- Indexes for table `suppliers`
--
ALTER TABLE `suppliers`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_suppliers_kodesupplier` (`KodeSupplier`);

--
-- Indexes for table `systemsettings`
--
ALTER TABLE `systemsettings`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `transaksistok`
--
ALTER TABLE `transaksistok`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_TransaksiStok_GudangId` (`GudangId`),
  ADD KEY `IX_TransaksiStok_SupplierId` (`SupplierId`),
  ADD KEY `IX_transaksistok_nomorreferensi` (`NomorReferensi`(50));

--
-- Indexes for table `transaksistokdetail`
--
ALTER TABLE `transaksistokdetail`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_TransaksiStokDetail_ProdukId` (`ProdukId`),
  ADD KEY `IX_TransaksiStokDetail_RakId` (`RakId`),
  ADD KEY `IX_TransaksiStokDetail_TransaksiStokId` (`TransaksiStokId`);

--
-- Indexes for table `transferorderdetails`
--
ALTER TABLE `transferorderdetails`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_TransferOrderDetails_ProdukId` (`ProdukId`),
  ADD KEY `IX_TransferOrderDetails_TransferOrderId` (`TransferOrderId`);

--
-- Indexes for table `transferorders`
--
ALTER TABLE `transferorders`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_TransferOrders_GudangAsalId` (`GudangAsalId`),
  ADD KEY `IX_TransferOrders_GudangTujuanId` (`GudangTujuanId`),
  ADD KEY `IX_transferorders_nomordokumen` (`NomorDokumen`);

--
-- Indexes for table `__efmigrationshistory`
--
ALTER TABLE `__efmigrationshistory`
  ADD PRIMARY KEY (`MigrationId`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `activitylogs`
--
ALTER TABLE `activitylogs`
  MODIFY `Id` bigint(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=49;

--
-- AUTO_INCREMENT for table `aspnetroleclaims`
--
ALTER TABLE `aspnetroleclaims`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=167;

--
-- AUTO_INCREMENT for table `aspnetuserclaims`
--
ALTER TABLE `aspnetuserclaims`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `customers`
--
ALTER TABLE `customers`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=71;

--
-- AUTO_INCREMENT for table `departments`
--
ALTER TABLE `departments`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=25;

--
-- AUTO_INCREMENT for table `gudang`
--
ALTER TABLE `gudang`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=54;

--
-- AUTO_INCREMENT for table `karyawans`
--
ALTER TABLE `karyawans`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=53;

--
-- AUTO_INCREMENT for table `kategoris`
--
ALTER TABLE `kategoris`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=22;

--
-- AUTO_INCREMENT for table `master_uom`
--
ALTER TABLE `master_uom`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=23;

--
-- AUTO_INCREMENT for table `menus`
--
ALTER TABLE `menus`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=39;

--
-- AUTO_INCREMENT for table `peminjamanaset`
--
ALTER TABLE `peminjamanaset`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=55;

--
-- AUTO_INCREMENT for table `peminjamanasetdetail`
--
ALTER TABLE `peminjamanasetdetail`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=55;

--
-- AUTO_INCREMENT for table `penugasanasetdetail`
--
ALTER TABLE `penugasanasetdetail`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=71;

--
-- AUTO_INCREMENT for table `penugasanasets`
--
ALTER TABLE `penugasanasets`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=71;

--
-- AUTO_INCREMENT for table `permissions`
--
ALTER TABLE `permissions`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=28;

--
-- AUTO_INCREMENT for table `produk`
--
ALTER TABLE `produk`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=60;

--
-- AUTO_INCREMENT for table `produk_item`
--
ALTER TABLE `produk_item`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=29;

--
-- AUTO_INCREMENT for table `produk_settings`
--
ALTER TABLE `produk_settings`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=52;

--
-- AUTO_INCREMENT for table `rak`
--
ALTER TABLE `rak`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=53;

--
-- AUTO_INCREMENT for table `reparasi_aset`
--
ALTER TABLE `reparasi_aset`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=53;

--
-- AUTO_INCREMENT for table `stockopname`
--
ALTER TABLE `stockopname`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=51;

--
-- AUTO_INCREMENT for table `stockopnamedetail`
--
ALTER TABLE `stockopnamedetail`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=57;

--
-- AUTO_INCREMENT for table `suppliers`
--
ALTER TABLE `suppliers`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=22;

--
-- AUTO_INCREMENT for table `systemsettings`
--
ALTER TABLE `systemsettings`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `transaksistok`
--
ALTER TABLE `transaksistok`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=58;

--
-- AUTO_INCREMENT for table `transaksistokdetail`
--
ALTER TABLE `transaksistokdetail`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT for table `transferorderdetails`
--
ALTER TABLE `transferorderdetails`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=53;

--
-- AUTO_INCREMENT for table `transferorders`
--
ALTER TABLE `transferorders`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=53;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `aspnetroleclaims`
--
ALTER TABLE `aspnetroleclaims`
  ADD CONSTRAINT `FK_AspNetRoleClaims_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `aspnetroles` (`Id`) ON DELETE CASCADE;

--
-- Constraints for table `aspnetuserclaims`
--
ALTER TABLE `aspnetuserclaims`
  ADD CONSTRAINT `FK_AspNetUserClaims_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE;

--
-- Constraints for table `aspnetuserlogins`
--
ALTER TABLE `aspnetuserlogins`
  ADD CONSTRAINT `FK_AspNetUserLogins_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE;

--
-- Constraints for table `aspnetuserroles`
--
ALTER TABLE `aspnetuserroles`
  ADD CONSTRAINT `FK_AspNetUserRoles_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `aspnetroles` (`Id`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_AspNetUserRoles_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE;

--
-- Constraints for table `aspnetusers`
--
ALTER TABLE `aspnetusers`
  ADD CONSTRAINT `FK_AspNetUsers_Karyawans_KaryawanId` FOREIGN KEY (`KaryawanId`) REFERENCES `karyawans` (`Id`);

--
-- Constraints for table `aspnetusertokens`
--
ALTER TABLE `aspnetusertokens`
  ADD CONSTRAINT `FK_AspNetUserTokens_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE;

--
-- Constraints for table `karyawans`
--
ALTER TABLE `karyawans`
  ADD CONSTRAINT `FK_Karyawans_Departments_DepartmentId` FOREIGN KEY (`DepartmentId`) REFERENCES `departments` (`Id`) ON DELETE CASCADE;

--
-- Constraints for table `peminjamanaset`
--
ALTER TABLE `peminjamanaset`
  ADD CONSTRAINT `FK_peminjamanaset_Karyawans_KaryawanId` FOREIGN KEY (`KaryawanId`) REFERENCES `karyawans` (`Id`);

--
-- Constraints for table `peminjamanasetdetail`
--
ALTER TABLE `peminjamanasetdetail`
  ADD CONSTRAINT `FK_peminjamanasetdetail_peminjamanaset_PeminjamanAsetID` FOREIGN KEY (`PeminjamanAsetID`) REFERENCES `peminjamanaset` (`Id`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_peminjamanasetdetail_produk_item_ProdukItemId` FOREIGN KEY (`ProdukItemId`) REFERENCES `produk_item` (`Id`) ON DELETE CASCADE;

--
-- Constraints for table `penugasanasetdetail`
--
ALTER TABLE `penugasanasetdetail`
  ADD CONSTRAINT `FK_penugasanasetdetail_PenugasanAsets_PenugasanAsetId` FOREIGN KEY (`PenugasanAsetId`) REFERENCES `penugasanasets` (`Id`) ON DELETE CASCADE;

--
-- Constraints for table `produk`
--
ALTER TABLE `produk`
  ADD CONSTRAINT `FK_Produk_Gudang_GudangId` FOREIGN KEY (`GudangId`) REFERENCES `gudang` (`Id`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_Produk_MasterUom_MasterUomId` FOREIGN KEY (`MasterUomId`) REFERENCES `master_uom` (`Id`) ON DELETE SET NULL,
  ADD CONSTRAINT `FK_Produk_Rak_RakId` FOREIGN KEY (`RakId`) REFERENCES `rak` (`Id`);

--
-- Constraints for table `produk_item`
--
ALTER TABLE `produk_item`
  ADD CONSTRAINT `FK_produk_item_Gudang_GudangId` FOREIGN KEY (`GudangId`) REFERENCES `gudang` (`Id`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_produk_item_Produk_ProdukId` FOREIGN KEY (`ProdukId`) REFERENCES `produk` (`Id`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_produk_item_Rak_RakId` FOREIGN KEY (`RakId`) REFERENCES `rak` (`Id`) ON DELETE CASCADE;

--
-- Constraints for table `produk_settings`
--
ALTER TABLE `produk_settings`
  ADD CONSTRAINT `FK_produk_settings_Gudang_gudang_id` FOREIGN KEY (`gudang_id`) REFERENCES `gudang` (`Id`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_produk_settings_Produk_produk_id` FOREIGN KEY (`produk_id`) REFERENCES `produk` (`Id`) ON DELETE CASCADE;

--
-- Constraints for table `rak`
--
ALTER TABLE `rak`
  ADD CONSTRAINT `FK_Rak_Gudang_GudangId` FOREIGN KEY (`GudangId`) REFERENCES `gudang` (`Id`) ON DELETE CASCADE;

--
-- Constraints for table `stockopname`
--
ALTER TABLE `stockopname`
  ADD CONSTRAINT `FK_StockOpname_Gudang_GudangId` FOREIGN KEY (`GudangId`) REFERENCES `gudang` (`Id`) ON DELETE CASCADE;

--
-- Constraints for table `stockopnamedetail`
--
ALTER TABLE `stockopnamedetail`
  ADD CONSTRAINT `FK_StockOpnameDetail_Produk_ProdukId` FOREIGN KEY (`ProdukId`) REFERENCES `produk` (`Id`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_StockOpnameDetail_StockOpname_StockOpnameId` FOREIGN KEY (`StockOpnameId`) REFERENCES `stockopname` (`Id`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_StockOpnameDetail_produk_item_ProdukItemId` FOREIGN KEY (`ProdukItemId`) REFERENCES `produk_item` (`Id`);

--
-- Constraints for table `transaksistok`
--
ALTER TABLE `transaksistok`
  ADD CONSTRAINT `FK_TransaksiStok_Gudang_GudangId` FOREIGN KEY (`GudangId`) REFERENCES `gudang` (`Id`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_TransaksiStok_Suppliers_SupplierId` FOREIGN KEY (`SupplierId`) REFERENCES `suppliers` (`Id`);

--
-- Constraints for table `transaksistokdetail`
--
ALTER TABLE `transaksistokdetail`
  ADD CONSTRAINT `FK_TransaksiStokDetail_Produk_ProdukId` FOREIGN KEY (`ProdukId`) REFERENCES `produk` (`Id`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_TransaksiStokDetail_Rak_RakId` FOREIGN KEY (`RakId`) REFERENCES `rak` (`Id`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_TransaksiStokDetail_TransaksiStok_TransaksiStokId` FOREIGN KEY (`TransaksiStokId`) REFERENCES `transaksistok` (`Id`) ON DELETE CASCADE;

--
-- Constraints for table `transferorderdetails`
--
ALTER TABLE `transferorderdetails`
  ADD CONSTRAINT `FK_TransferOrderDetails_Produk_ProdukId` FOREIGN KEY (`ProdukId`) REFERENCES `produk` (`Id`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_TransferOrderDetails_TransferOrders_TransferOrderId` FOREIGN KEY (`TransferOrderId`) REFERENCES `transferorders` (`Id`) ON DELETE CASCADE;

--
-- Constraints for table `transferorders`
--
ALTER TABLE `transferorders`
  ADD CONSTRAINT `FK_TransferOrders_Gudang_GudangAsalId` FOREIGN KEY (`GudangAsalId`) REFERENCES `gudang` (`Id`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_TransferOrders_Gudang_GudangTujuanId` FOREIGN KEY (`GudangTujuanId`) REFERENCES `gudang` (`Id`) ON DELETE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
SET FOREIGN_KEY_CHECKS = 1;