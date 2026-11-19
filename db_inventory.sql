-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Nov 19, 2026 at 08:15 AM
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

-- Matikan sementara foreign key checks untuk proses truncate & insert ulang tabel
SET FOREIGN_KEY_CHECKS = 0;

-- --------------------------------------------------------
-- TRUNCATE TABLE (Urutan dari tabel anak/detail ke tabel induk/utama)
-- --------------------------------------------------------
TRUNCATE TABLE `ActivityLogs`;
TRUNCATE TABLE `AspNetRoleClaims`;
TRUNCATE TABLE `AspNetUserClaims`;
TRUNCATE TABLE `aspnetuserlogins`;
TRUNCATE TABLE `AspNetUserRoles`;
TRUNCATE TABLE `aspnetusertokens`;
TRUNCATE TABLE `peminjamanasetdetail`;
TRUNCATE TABLE `penugasanasetdetail`;
TRUNCATE TABLE `StockOpnameDetail`;
TRUNCATE TABLE `TransaksiStokDetail`;
TRUNCATE TABLE `TransferOrderDetails`;
TRUNCATE TABLE `produk_item`;
TRUNCATE TABLE `produk_settings`;
TRUNCATE TABLE `peminjamanaset`;
TRUNCATE TABLE `PenugasanAsets`;
TRUNCATE TABLE `reparasi_aset`;
TRUNCATE TABLE `StockOpname`;
TRUNCATE TABLE `TransaksiStok`;
TRUNCATE TABLE `TransferOrders`;
TRUNCATE TABLE `Produk`;
TRUNCATE TABLE `Karyawans`;
TRUNCATE TABLE `AspNetUsers`;
TRUNCATE TABLE `AspNetRoles`;
TRUNCATE TABLE `Customers`;
TRUNCATE TABLE `Departments`;
TRUNCATE TABLE `Gudang`;
TRUNCATE TABLE `Kategoris`;
TRUNCATE TABLE `master_uom`;
TRUNCATE TABLE `Menus`;
TRUNCATE TABLE `Permissions`;
TRUNCATE TABLE `Rak`;
TRUNCATE TABLE `Suppliers`;
TRUNCATE TABLE `SystemSettings`;
-- --------------------------------------------------------

--
-- Dumping data for table `ActivityLogs`
--

INSERT INTO `ActivityLogs` (`Id`, `Causer`, `Event`, `Description`, `Subject`, `Timestamp`) VALUES
(1, 'Cindy Lestari', 'UPDATE', 'Melakukan aksi UPDATE pada data Produk (ID/Ref: 9)', 'PRODUK', '2026-11-15 02:42:45.961586'),
(2, 'Budi Santoso', 'UPDATE', 'Melakukan aksi UPDATE pada data PenugasanAsets (ID/Ref: 19)', 'PenugasanAsets', '2026-11-15 09:43:37.184885'),
(3, 'Budi Santoso', 'UPDATE', 'Melakukan aksi UPDATE pada data penugasanasetdetail (ID/Ref: 19)', 'PENUGASANASETDETAIL', '2026-11-15 09:43:37.185026'),
(4, 'Budi Santoso', 'UPDATE', 'Melakukan aksi UPDATE pada data produk_item (ID/Ref: 1)', 'produk_item', '2026-11-15 09:43:37.185037'),
(8, 'Budi Santoso', 'UPDATE', 'Melakukan aksi UPDATE pada data PenugasanAsets (ID/Ref: 20)', 'PenugasanAsets', '2026-11-15 09:45:57.785960');

-- --------------------------------------------------------

--
-- Dumping data for table `AspNetRoles`
--

INSERT INTO `AspNetRoles` (`Id`, `Name`, `NormalizedName`, `ConcurrencyStamp`) VALUES
('0b1198d7-f735-40c7-97b2-29e017c27865', 'Super Admin', 'SUPER ADMIN', NULL),
('3f27ffeb-2740-4db6-8fd4-4986497b7160', 'Manager', 'MANAGER', '592debbc-74f3-4282-8cb1-3b765aaaf113'),
('4e3b18ad-18f7-415e-9ef9-3349dff3efe1', 'User', 'USER', 'a858ce25-6652-4874-9239-32cc750f76d0'),
('528be043-74b1-4c1d-8791-c330ec9cc421', 'Warehouse Manager', 'WAREHOUSE MANAGER', '61af62b9-fc60-4500-9ab3-6d333a5a0a08'),
('af74d88d-ccb0-4b55-b98b-142fea17d95c', 'Warehouse Staff', 'WAREHOUSE STAFF', '715b7ade-1b7e-40df-ac2d-63879c13eab2'),
('db442a01-3da5-471a-b6fb-44c47346902f', 'Admin', 'ADMIN', '873e4471-cbc2-43af-8e59-9347e252247f');

-- --------------------------------------------------------

--
-- Dumping data for table `AspNetRoleClaims`
--

INSERT INTO `AspNetRoleClaims` (`Id`, `RoleId`, `ClaimType`, `ClaimValue`) VALUES
(1, '0b1198d7-f735-40c7-97b2-29e017c27865', 'Permission', 'view dashboard'),
(2, '0b1198d7-f735-40c7-97b2-29e017c27865', 'Permission', 'manage users'),
(3, '0b1198d7-f735-40c7-97b2-29e017c27865', 'Permission', 'manage settings'),
(4, '0b1198d7-f735-40c7-97b2-29e017c27865', 'Permission', 'view logs'),
(5, '0b1198d7-f735-40c7-97b2-29e017c27865', 'Permission', 'manage warehouses'),
(6, '0b1198d7-f735-40c7-97b2-29e017c27865', 'Permission', 'manage products'),
(7, '0b1198d7-f735-40c7-97b2-29e017c27865', 'Permission', 'manage Suppliers'),
(8, '0b1198d7-f735-40c7-97b2-29e017c27865', 'Permission', 'manage stock opname'),
(9, '0b1198d7-f735-40c7-97b2-29e017c27865', 'Permission', 'manage transfer orders'),
(10, '0b1198d7-f735-40c7-97b2-29e017c27865', 'Permission', 'view stock matrix'),
(11, '0b1198d7-f735-40c7-97b2-29e017c27865', 'Permission', 'view stock cards'),
(12, '0b1198d7-f735-40c7-97b2-29e017c27865', 'Permission', 'manage Departments'),
(13, '0b1198d7-f735-40c7-97b2-29e017c27865', 'Permission', 'manage employees'),
(14, '0b1198d7-f735-40c7-97b2-29e017c27865', 'Permission', 'manage assignments'),
(15, '0b1198d7-f735-40c7-97b2-29e017c27865', 'Permission', 'manage loans'),
(16, '0b1198d7-f735-40c7-97b2-29e017c27865', 'Permission', 'view user guide'),
(17, '0b1198d7-f735-40c7-97b2-29e017c27865', 'Permission', 'view reparasi asset'),
(18, '0b1198d7-f735-40c7-97b2-29e017c27865', 'Permission', 'manage customer'),
(19, '0b1198d7-f735-40c7-97b2-29e017c27865', 'Permission', 'manage categories'),
(20, '0b1198d7-f735-40c7-97b2-29e017c27865', 'Permission', 'view asset reports'),
(21, '0b1198d7-f735-40c7-97b2-29e017c27865', 'Permission', 'view stock in/out reports'),
(22, '0b1198d7-f735-40c7-97b2-29e017c27865', 'Permission', 'manage uom'),
(23, '0b1198d7-f735-40c7-97b2-29e017c27865', 'Permission', 'manage racks'),
(24, '0b1198d7-f735-40c7-97b2-29e017c27865', 'Permission', 'manage reparasi aset'),
(25, '0b1198d7-f735-40c7-97b2-29e017c27865', 'Permission', 'manage roles and Permissions'),
(26, '0b1198d7-f735-40c7-97b2-29e017c27865', 'Permission', 'manage stock transactions'),
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
(67, 'af74d88d-ccb0-4b55-b98b-142fea17d95c', 'Permission', 'manage Suppliers'),
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
(86, '528be043-74b1-4c1d-8791-c330ec9cc421', 'Permission', 'manage Suppliers'),
(87, '528be043-74b1-4c1d-8791-c330ec9cc421', 'Permission', 'manage transfer orders'),
(88, '528be043-74b1-4c1d-8791-c330ec9cc421', 'Permission', 'manage employees'),
(89, '528be043-74b1-4c1d-8791-c330ec9cc421', 'Permission', 'manage loans'),
(90, '528be043-74b1-4c1d-8791-c330ec9cc421', 'Permission', 'view reparasi asset'),
(91, '528be043-74b1-4c1d-8791-c330ec9cc421', 'Permission', 'manage assignments'),
(92, '528be043-74b1-4c1d-8791-c330ec9cc421', 'Permission', 'manage Departments'),
(93, '528be043-74b1-4c1d-8791-c330ec9cc421', 'Permission', 'manage stock opname'),
(94, '528be043-74b1-4c1d-8791-c330ec9cc421', 'Permission', 'manage products'),
(95, '3f27ffeb-2740-4db6-8fd4-4986497b7160', 'Permission', 'view dashboard'),
(96, '3f27ffeb-2740-4db6-8fd4-4986497b7160', 'Permission', 'view stock matrix'),
(97, '3f27ffeb-2740-4db6-8fd4-4986497b7160', 'Permission', 'view user guide'),
(98, '3f27ffeb-2740-4db6-8fd4-4986497b7160', 'Permission', 'view stock mutations'),
(99, '3f27ffeb-2740-4db6-8fd4-4986497b7160', 'Permission', 'view logs'),
(100, '3f27ffeb-2740-4db6-8fd4-4986497b7160', 'Permission', 'manage warehouses'),
(101, '3f27ffeb-2740-4db6-8fd4-4986497b7160', 'Permission', 'manage Suppliers'),
(102, '3f27ffeb-2740-4db6-8fd4-4986497b7160', 'Permission', 'manage transfer orders'),
(103, '3f27ffeb-2740-4db6-8fd4-4986497b7160', 'Permission', 'manage employees'),
(104, '3f27ffeb-2740-4db6-8fd4-4986497b7160', 'Permission', 'manage loans'),
(105, '3f27ffeb-2740-4db6-8fd4-4986497b7160', 'Permission', 'view reparasi asset'),
(106, '3f27ffeb-2740-4db6-8fd4-4986497b7160', 'Permission', 'manage assignments'),
(107, '3f27ffeb-2740-4db6-8fd4-4986497b7160', 'Permission', 'manage Departments'),
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
(122, '4e3b18ad-18f7-415e-9ef9-3349dff3efe1', 'Permission', 'manage Departments'),
(123, '4e3b18ad-18f7-415e-9ef9-3349dff3efe1', 'Permission', 'manage Suppliers'),
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
(146, 'db442a01-3da5-471a-b6fb-44c47346902f', 'Permission', 'manage Departments'),
(147, 'db442a01-3da5-471a-b6fb-44c47346902f', 'Permission', 'manage Suppliers'),
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
-- Dumping data for table `Customers`
--

INSERT INTO `Customers` (`Id`, `KodeCustomer`, `NamaCustomer`, `Email`, `NoTelepon`, `Alamat`, `IsActive`) VALUES
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
(20, 'CUST-020', 'PT Apex Global Solusi', 'apex@globalsolusi.co.id', '021-2998877', 'Grand Indonesia West Mall, Jakarta Pusat', 1);

-- --------------------------------------------------------

--
-- Dumping data for table `Departments`
--

INSERT INTO `Departments` (`Id`, `Code`, `Name`, `Description`, `CreatedAt`, `UpdatedAt`) VALUES
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
-- Dumping data for table `Gudang`
--

INSERT INTO `Gudang` (`Id`, `KodeGudang`, `NamaGudang`, `TipeGudang`, `PicGudang`, `AlamatGudang`, `IsActive`, `IsAudit`, `CreatedAt`, `UpdatedAt`) VALUES
(1, 'GDG-JKT-1', 'Gudang Utama Jakarta', 'fisik', 'Budi Santoso', 'Jl. Industri No. 12, Jakarta Utara', 1, 0, '2026-10-30 15:52:54.416675', '2026-11-08 13:46:28.473474'),
(2, 'GDG-SBY-01', 'Gudang Cabang Surabaya', 'fisik', 'Santoso', 'Kawasan Industri Rungkut Block B-3, Surabaya', 1, 0, '2026-11-01 09:49:25.062669', NULL),
(3, 'GDG-VIR-1', 'Gudang Transit Virtual', 'virtual', 'Sistem Otomatis', 'Cloud Database System', 1, 0, '2026-11-05 09:19:16.896040', NULL);

-- --------------------------------------------------------

--
-- Dumping data for table `Karyawans`
--

INSERT INTO `Karyawans` (`Id`, `Nik`, `NamaLengkap`, `Email`, `NoTelepon`, `DepartmentId`, `Status`, `Jabatan`, `UrlFoto`) VALUES
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
-- Dumping data for table `AspNetUsers`
--

INSERT INTO `AspNetUsers` (`Id`, `UserName`, `NormalizedUserName`, `Email`, `NormalizedEmail`, `EmailConfirmed`, `PasswordHash`, `SecurityStamp`, `ConcurrencyStamp`, `PhoneNumber`, `PhoneNumberConfirmed`, `TwoFactorEnabled`, `LockoutEnd`, `LockoutEnabled`, `AccessFailedCount`, `CreatedAt`, `FullName`, `KaryawanId`) VALUES
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
-- Dumping data for table `AspNetUserRoles`
--

INSERT INTO `AspNetUserRoles` (`UserId`, `RoleId`) VALUES
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
-- Dumping data for table `Kategoris`
--

INSERT INTO `Kategoris` (`Id`, `NamaKategori`, `Deskripsi`) VALUES
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
(20, 'Material Logistik', 'Bahan baku dan kemasan Gudang');

-- --------------------------------------------------------

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
-- Dumping data for table `Menus`
--

INSERT INTO `Menus` (`Id`, `MenuTitle`, `Url`, `GroupName`, `ParentMenu`, `SortOrder`, `IconClass`, `RequireRoles`, `RequirePermissions`) VALUES
(8, 'Dashboard', '/Dashboard/Index', 'Main Menu', NULL, 1, 'bi-speedometer2', 'Super Admin,Manager,User,Warehouse Manager,Warehouse Staff,Admin', 'view dashboard'),
(10, 'Panduan Pengguna', '/Guide/Index', 'Main Menu', NULL, 2, 'bi-book', 'Super Admin,Manager,User,Warehouse Manager,Warehouse Staff,Admin', 'view user guide'),
(12, 'Manajemen Gudang', '/Warehouses/Index', 'Inventory Management', NULL, 1, 'bi-building', 'Super Admin,Manager,User,Warehouse Manager,Warehouse Staff,Admin', 'manage warehouses'),
(13, 'Katalog Produk', '/Produk/Index', 'Inventory Management', NULL, 5, 'bi-box-seam', 'Super Admin,Manager,User,Warehouse Manager,Warehouse Staff,Admin', 'manage products'),
(14, 'Supplier', '/Supplier/Index', 'Inventory Management', NULL, 6, 'bi-shop', 'Super Admin,Manager,User,Warehouse Manager,Warehouse Staff,Admin', 'manage Suppliers'),
(15, 'Karyawan', '/Karyawan/Index', 'Inventory Management', NULL, 7, 'bi-people', 'Super Admin,Manager,User,Warehouse Manager,Admin', 'manage employees'),
(16, 'Departemen', '/Department/Index', 'Inventory Management', NULL, 8, 'bi-building', 'Super Admin,Manager,User,Warehouse Manager,Admin', 'manage Departments'),
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
(30, 'Roles & Permissions', '/RolesPermissions/Index', 'Settings', NULL, 3, 'bi-shield-check', 'Super Admin', 'manage roles and Permissions'),
(31, 'Menu Manager', '/MenuManager/Index', 'Settings', NULL, 4, 'bi-list', 'Super Admin', 'manage settings'),
(34, 'Activity Logs', '/ActivityLogs/Index', 'Other', NULL, 1, 'bi-clock-history', 'Super Admin', 'view logs'),
(35, 'Kategori', '/Kategori/Index', 'Inventory Management', NULL, 4, 'bi-box-seam', 'Super Admin,Manager,User,Warehouse Manager,Admin', 'manage categories'),
(36, 'UOM', '/MasterUom/Index', 'Inventory Management', NULL, 3, 'bi-file-earmark-text', 'Super Admin,Manager,User,Warehouse Manager,Admin', 'manage uom'),
(37, 'Rak', '/Raks/Index', 'Inventory Management', NULL, 2, 'bi-book', 'Super Admin,Manager,User,Warehouse Manager,Admin', 'manage racks'),
(38, 'Reparasi Assets', '/ReparasiAsets/Index', 'Asset Tracking', NULL, 3, 'bi-clipboard-check', 'Super Admin,Manager,User,Warehouse Manager,Warehouse Staff,Admin', 'manage reparasi aset');

-- --------------------------------------------------------

--
-- Dumping data for table `Permissions`
--

INSERT INTO `Permissions` (`Id`, `Name`, `Category`, `Type`, `Description`) VALUES
(1, 'view dashboard', 'Navigation & View', 'System Core', NULL),
(2, 'manage users', 'User Management', 'System Core', NULL),
(3, 'manage settings', 'System Settings', 'System Core', NULL),
(4, 'view logs', 'Audit Logs', 'System Core', NULL),
(5, 'manage warehouses', 'General', 'Custom', NULL),
(6, 'manage products', 'General', 'Custom', NULL),
(7, 'manage Suppliers', 'General', 'Custom', NULL),
(8, 'manage stock opname', 'General', 'Custom', NULL),
(9, 'manage transfer orders', 'General', 'Custom', NULL),
(10, 'view stock matrix', 'Navigation & View', 'Custom', NULL),
(11, 'view stock cards', 'Navigation & View', 'Custom', NULL),
(12, 'manage Departments', 'General', 'Custom', NULL),
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
(26, 'manage roles and Permissions', 'System Settings', 'System Core', NULL),
(27, 'manage stock transactions', 'General', 'Custom', NULL);

-- --------------------------------------------------------

--
-- Dumping data for table `Rak`
--

INSERT INTO `Rak` (`Id`, `Area`, `NamaRak`, `BinNo`, `KodeLokasi`, `Deskripsi`, `IsActive`, `GudangId`) VALUES
(1, 'Zone A (Electronics)', 'Rak A - Baris 1', 'Bin 01', 'A-01-01', 'Rak khusus penyimpanan laptop dan smartphone.', 1, 1),
(2, 'Zone A (Electronics)', 'Rak B - Baris 1', 'Bin 01', 'B-01-01', 'Kawasan Industri Rungkut Block B-3, Surabaya', 1, 2);

-- --------------------------------------------------------

--
-- Dumping data for table `Suppliers`
--

INSERT INTO `Suppliers` (`Id`, `KodeSupplier`, `NamaSupplier`, `Email`, `Telepon`, `NamaPic`, `WhatsappPic`, `Alamat`, `Catatan`, `IsAktif`) VALUES
(1, 'SPL-001', 'PT. Global Teknologi Indonesia', 'info@globaltech.co.id', '021-5558989', 'Andi Wijaya', '081234567890', 'Gedung Cyber Lt. 5, Jl. Kuningan Barat, Jakarta Selatan', 'Pemasok utama perangkat IT, server, dan jaringan.', 1),
(2, 'SPL-002', 'CV. Maju Bersama Logistik', 'sales@majubersama.com', '031-7772323', 'Budi Setiawan', '082199887766', 'Kawasan Industri SIER Blok F-12, Surabaya', 'Pemasok furniture kantor dan alat-alat Gudang.', 1),
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
-- Dumping data for table `Produk`
--

INSERT INTO `Produk` (`Id`, `PelacakanBarang`, `Sku`, `NamaProduk`, `Kategori`, `Merek`, `Satuan`, `TipeBarang`, `HargaBeli`, `HargaPerolehan`, `Deskripsi`, `Stok`, `GudangId`, `RakId`, `MasterUomId`) VALUES
(1, 'Pelacakan Serial Number (SN)', 'SKU-ITE-0001', 'Laptop Dell Latitude 5440 i7', 'IT Equipment', 'Dell', 'Unit', 'Asset', 16500000.00, 16500000.00, 'Laptop Dell Latitude 5440, Intel Core i7, 16GB RAM, 512GB SSD.', 0, 1, 1, NULL),
(2, 'Pelacakan Serial Number (SN)', 'SKU-ITE-0002', 'Laptop Lenovo ThinkPad L14', 'IT Equipment', 'Lenovo', 'Unit', 'Inventory (Komersial)', 14500000.00, 14500000.00, 'Laptop Lenovo ThinkPad L14 Gen 4, AMD Ryzen 5, 16GB RAM, 512GB SSD.', 1, 1, 1, NULL),
(3, 'Tanpa Serial Number (Bulk)', 'SKU-CON-0001', 'Kertas A4 80gsm PaperOne', 'Consumables', '-', 'Box', 'Consumable (Habis Pakai)', 245000.00, 245000.00, 'Photocopy paper A4 size 80gsm, 1 box = 5 reams.', 10, 1, 1, NULL),
(6, 'Pelacakan Serial Number (SN)', 'SKU-ITE-0003', 'Laptop Dell Latitude 5440 i7', 'IT Equipment', 'Dell', 'Unit', 'Asset', 16500000.00, 16500000.00, 'Laptop Dell Latitude 5440, Intel Core i7, 16GB RAM, 512GB SSD.', 20, 2, 2, NULL),
(7, 'Tanpa Serial Number (Bulk)', 'SKU-ITE-0004', 'Laptop Dell Latitude 5440 i7', 'IT Equipment', 'Dell', 'Unit', 'Inventory (Komersial)', 15600000.00, 15600000.00, 'Laptop Dell Latitude 5440, Intel Core i7, 16GB RAM, 512GB SSD.', 15, 3, NULL, NULL),
(9, 'Tanpa Serial Number (Bulk)', 'SKU-ITE-0005', 'Laptop Dell Latitude 5440 i7', 'IT Equipment', 'Dell', 'Unit', 'Inventory (Komersial)', 15600000.00, 15600000.00, '-', 20, 1, 1, 2);

-- --------------------------------------------------------

--
-- Dumping data for table `produk_item`
--

INSERT INTO `produk_item` (`Id`, `ProdukId`, `GudangId`, `RakId`, `SerialNumber`, `Status`, `QrCodePath`, `CreatedAt`) VALUES
(1, 1, 1, 1, 'ITE-0001-GDG-JKT-1-0001', 'Assigned', '/qrcodes/ITE-0001-GDG-JKT-1-0001.png', '2026-10-30 15:56:47.660465'),
(2, 1, 1, 1, 'ITE-0001-GDG-JKT-1-0002', 'Sold', '/qrcodes/ITE-0001-GDG-JKT-1-0002.png', '2026-10-30 15:56:47.696004'),
(4, 2, 1, 1, 'ITE-0002-GDG-JKT-1-0002', 'Dipinjam', '/qrcodes/ITE-0002-GDG-JKT-1-0002.png', '2026-10-31 09:59:30.077557'),
(5, 2, 1, 1, 'ITE-0002-GDG-JKT-1-0003', 'Lost', '/qrcodes/ITE-0002-GDG-JKT-1-0003.png', '2026-10-31 09:59:30.082883'),
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
-- Dumping data for table `produk_settings`
--

INSERT INTO `produk_settings` (`Id`, `Produk_id`, `Gudang_id`, `safety_stock`) VALUES
(1, 3, 1, 20);

-- --------------------------------------------------------

--
-- Dumping data for table `peminjamanaset`
--

INSERT INTO `peminjamanaset` (`Id`, `KodePeminjaman`, `TanggalPinjam`, `TanggalJatuhTempo`, `TanggalPengembalian`, `TujuanPeminjaman`, `JumlahHariPinjam`, `JumlahHariTelatPinjam`, `Status`, `GudangId`, `KaryawanId`) VALUES
(1, 'LN-202610-0001', '2026-10-30 00:00:00.000000', '2026-10-30 00:00:00.000000', '2026-11-01 00:00:00.000000', NULL, 0, 0, 1, 1, 1),
(2, 'LN-202610-0002', '2026-10-30 00:00:00.000000', '2026-10-30 00:00:00.000000', '2026-10-30 00:00:00.000000', NULL, 0, 0, 2, 1, 1),
(3, 'LN-202610-0003', '2026-10-31 00:00:00.000000', '2026-10-31 00:00:00.000000', NULL, NULL, 0, 0, 1, 1, 2),
(4, 'LN-202611-0001', '2026-11-01 00:00:00.000000', '2026-11-01 00:00:00.000000', NULL, NULL, 0, 0, 1, 1, 2);

-- --------------------------------------------------------

--
-- Dumping data for table `peminjamanasetdetail`
--

INSERT INTO `peminjamanasetdetail` (`Id`, `PeminjamanAsetID`, `ProdukItemId`, `NamaBarang`, `SerialNumber`, `LokasiAwal`, `Kuantitas`, `KondisiSerah`, `KondisiKembali`) VALUES
(1, 1, 2, 'Laptop Dell Latitude 5440 i7', 'ITE-0001-GDG-JKT-1-0002', 'Gudang Utama Jakarta', 1, 'Good', 'Broken'),
(2, 2, 2, 'Laptop Dell Latitude 5440 i7', 'ITE-0001-GDG-JKT-1-0002', 'Gudang Utama Jakarta', 1, 'Good', 'Good'),
(3, 3, 1, 'Laptop Dell Latitude 5440 i7', 'ITE-0001-GDG-JKT-1-0001', 'Gudang Utama Jakarta', 1, 'Good', NULL),
(4, 4, 4, 'Laptop Lenovo ThinkPad L14', 'ITE-0002-GDG-JKT-1-0002', 'Gudang Utama Jakarta', 1, 'Good', NULL);

-- --------------------------------------------------------

--
-- Dumping data for table `PenugasanAsets`
--

INSERT INTO `PenugasanAsets` (`Id`, `KodePenugasan`, `TipePenerima`, `TargetPenerimaId`, `TanggalSerahTerima`, `TanggalPengembalian`, `Keterangan`, `Status`) VALUES
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
(20, 'AS-202611-0020', 1, 37, '2026-11-15 00:00:00.000000', NULL, NULL, 2);

-- --------------------------------------------------------

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
(20, 20, 'Laptop Dell Latitude 5440 i7', 'ITE-0001-GDG-JKT-1-0001', 'Gudang Utama Jakarta', 1, 'Good', NULL);

-- --------------------------------------------------------

--
-- Dumping data for table `reparasi_aset`
--

INSERT INTO `reparasi_aset` (`Id`, `ProdukItemId`, `TanggalMasuk`, `TanggalKeluar`, `Keluhan`, `Biaya`, `StatusReparasi`, `KondisiBarang`) VALUES
(2, 5, '2026-10-31 00:00:00.000000', '2026-11-07 00:00:00.000000', 'Ganti Battrey', 650000.00, 1, 'Rusak');

-- --------------------------------------------------------

--
-- Dumping data for table `StockOpname`
--

INSERT INTO `StockOpname` (`Id`, `NoReferensi`, `GudangId`, `Auditor`, `TanggalMulai`, `TanggalSelesai`, `Status`, `Catatan`) VALUES
(1, 'SO-202611-0001', 1, 'Super Admin', '2026-11-01 09:02:49.510864', '2026-11-01 09:16:13.540086', 1, '');

-- --------------------------------------------------------

--
-- Dumping data for table `StockOpnameDetail`
--

INSERT INTO `StockOpnameDetail` (`Id`, `StockOpnameId`, `ProdukId`, `ProdukItemId`, `StokSistem`, `StokFisik`, `AlasanSelisih`, `Keterangan`, `Kondisi`, `SerialNumber`) VALUES
(1, 1, 1, 1, 2, 0, '', '', 'Hilang (Lost)', 'ITE-0001-GDG-JKT-1-0001'),
(2, 1, 1, 2, 2, 1, '', '', 'Ditemukan (OK)', 'ITE-0001-GDG-JKT-1-0002'),
(3, 1, 2, 4, 3, 1, '', '', 'Ditemukan (OK)', 'ITE-0002-GDG-JKT-1-0002'),
(5, 1, 2, 7, 3, 1, '', '', 'Ditemukan (OK)', 'ITE-0002-GDG-JKT-1-0005'),
(6, 1, 3, NULL, 10, 10, '', '', 'Ditemukan (OK)', NULL);

-- --------------------------------------------------------

--
-- Dumping data for table `SystemSettings`
--

INSERT INTO `SystemSettings` (`Id`, `SiteName`, `SiteDescription`, `CompanyAddress`, `CompanyPhone`, `CompanyEmail`) VALUES
(1, 'PT. CONTOH MAJU JAYA', 'Inventory Multi-Gudang Modern. Sinkronisasi stok antar-cabang, pantau valuasi aset, dan kelola mutasi barang presisi.', 'Komp. Perkantoran Indah Blok B-12, Jakarta Selatan, Indonesia', '(021) 555-9830', 'support@ptcontohmajujaya.com');

-- --------------------------------------------------------

--
-- Dumping data for table `TransaksiStok`
--

INSERT INTO `TransaksiStok` (`Id`, `NomorReferensi`, `GudangId`, `SupplierId`, `TipeTransaksi`, `Catatan`, `Pembuat`, `TanggalPosting`, `IsPenjualan`) VALUES
(1, 'TRX-IN-20261031163017', 1, 1, 'Stock In', 'Penambahan Stock', 'Super Admin', '2026-10-31 16:30:17.104906', 0),
(2, 'TRX-OUT-20261031163206', 1, NULL, 'Stock Out', 'Penjuanlan barang', 'Super Admin', '2026-10-31 16:32:06.266880', 1),
(7, 'TRX-OUT-20261101092149', 1, NULL, 'Stock Out', NULL, 'System', '2026-11-01 09:21:49.623562', 1);

-- --------------------------------------------------------

--
-- Dumping data for table `TransaksiStokDetail`
--

INSERT INTO `TransaksiStokDetail` (`Id`, `TransaksiStokId`, `ProdukId`, `RakId`, `Jumlah`, `SerialNumbers`) VALUES
(1, 1, 2, 1, 1, NULL),
(2, 1, 1, 1, 1, NULL),
(3, 2, 2, 1, 1, 'ITE-0002-GDG-JKT-1-0001'),
(8, 7, 1, 1, 1, 'ITE-0001-GDG-JKT-1-0002');

-- --------------------------------------------------------

--
-- Dumping data for table `TransferOrders`
--

INSERT INTO `TransferOrders` (`Id`, `NomorDokumen`, `GudangAsalId`, `GudangTujuanId`, `Catatan`, `Status`, `TanggalDibuat`, `TanggalDikirim`, `TanggalDiterima`, `Pembuat`) VALUES
(1, 'TO/20261101/0001', 1, 2, 'Pindah asset ke Gudang surabaya.', 2, '2026-11-01 09:55:49.378928', NULL, '2026-11-01 09:55:56.489880', ''),
(2, 'TO/20261101/0002', 1, 2, '', 2, '2026-11-01 16:32:57.858538', NULL, '2026-11-01 16:34:13.549836', '');

-- --------------------------------------------------------

--
-- Dumping data for table `TransferOrderDetails`
--

INSERT INTO `TransferOrderDetails` (`Id`, `TransferOrderId`, `ProdukId`, `KuantitasPermintaan`, `KuantitasDiterima`) VALUES
(1, 1, 2, 1, 1),
(2, 2, 2, 1, 1);

-- --------------------------------------------------------
SET FOREIGN_KEY_CHECKS = 1;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
