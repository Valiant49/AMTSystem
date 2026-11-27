-- phpMyAdmin SQL Dump
-- version 5.2.3
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Nov 27, 2026 at 07:04 AM
-- Server version: 10.11.16-MariaDB
-- PHP Version: 8.3.36

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `amtsystem`
--

-- --------------------------------------------------------

--
-- Table structure for table `account_statuses`
--

CREATE TABLE `account_statuses` (
  `id` int(11) NOT NULL,
  `status` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `account_statuses`
--

INSERT INTO `account_statuses` (`id`, `status`) VALUES
(1, 'Active'),
(2, 'Inactive'),
(3, 'Suspended');

-- --------------------------------------------------------

--
-- Table structure for table `assets`
--

CREATE TABLE `assets` (
  `id` int(11) NOT NULL,
  `name` varchar(100) NOT NULL,
  `tag` varchar(50) NOT NULL,
  `category_id` int(11) NOT NULL,
  `parent_asset_id` int(11) DEFAULT NULL,
  `status_id` int(11) NOT NULL DEFAULT 1,
  `condition_id` int(11) NOT NULL DEFAULT 1,
  `description` text DEFAULT NULL,
  `serial_number` varchar(100) DEFAULT NULL,
  `components` text DEFAULT NULL,
  `manufacturer` varchar(100) NOT NULL,
  `model` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `assets`
--

INSERT INTO `assets` (`id`, `name`, `tag`, `category_id`, `parent_asset_id`, `status_id`, `condition_id`, `description`, `serial_number`, `components`, `manufacturer`, `model`) VALUES
(1, 'System Unit', 'AST-001', 2, NULL, 3, 2, 'Main computer system unit', 'SN-001', 'RAM, HDD, Fan', 'Dell', 'abc'),
(2, 'Motherboard', 'AST-002', 2, NULL, 1, 2, 'System unit motherboard', 'SN-002', NULL, 'AMD', 'def'),
(3, 'RAM', 'AST-003', 2, NULL, 1, 2, 'System unit memory', 'SN-003', NULL, 'Samsung', 'ghi'),
(4, 'Storage', 'AST-004', 2, NULL, 1, 2, 'System unit storage', 'SN-004', NULL, 'SanDisk', 'mno'),
(5, 'Test Laptop', 'TST-001', 1, NULL, 1, 2, NULL, 'SN-005', NULL, 'Dell', 'mno'),
(6, 'Test Monitor', 'TST-002', 1, NULL, 1, 2, NULL, 'SN-006', NULL, 'HP', 'xyz'),
(8, 'Acer Nitro Laptop', 'AST-0008', 2, NULL, 2, 2, 'laptop', '1234567890', 'something', 'Acer', 'ANV-15-41'),
(19, 'phone', '93526962', 4, NULL, 2, 4, 'phone', '12345678901', 'phone1', 'Tecno', 'KJ5'),
(20, 'something', '46025178', 2, NULL, 1, 2, 'jsndkjns', '12345678jkjn', 'something1', 'Dell', 'mno');

-- --------------------------------------------------------

--
-- Table structure for table `asset_categories`
--

CREATE TABLE `asset_categories` (
  `id` int(11) NOT NULL,
  `category` varchar(100) NOT NULL,
  `parent_category_id` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `asset_categories`
--

INSERT INTO `asset_categories` (`id`, `category`, `parent_category_id`) VALUES
(1, 'Hardware', NULL),
(2, 'Computer', 1),
(3, 'Printers', 1),
(4, 'Phones', 1);

-- --------------------------------------------------------

--
-- Table structure for table `asset_conditions`
--

CREATE TABLE `asset_conditions` (
  `id` int(11) NOT NULL,
  `condition` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `asset_conditions`
--

INSERT INTO `asset_conditions` (`id`, `condition`) VALUES
(1, 'New'),
(2, 'Good'),
(3, 'Fair'),
(4, 'Poor'),
(5, 'Damaged');

-- --------------------------------------------------------

--
-- Table structure for table `asset_statuses`
--

CREATE TABLE `asset_statuses` (
  `id` int(11) NOT NULL,
  `status` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `asset_statuses`
--

INSERT INTO `asset_statuses` (`id`, `status`) VALUES
(1, 'Available'),
(2, 'Issued'),
(3, 'Lent'),
(4, 'Maintenance'),
(5, 'Disposed');

-- --------------------------------------------------------

--
-- Table structure for table `asset_transactions`
--

CREATE TABLE `asset_transactions` (
  `id` int(11) NOT NULL,
  `batch_id` int(11) NOT NULL,
  `user_id` int(11) NOT NULL,
  `personnel_id` int(11) NOT NULL,
  `asset_id` int(11) NOT NULL,
  `transaction_date` timestamp NOT NULL DEFAULT current_timestamp(),
  `remarks` text DEFAULT NULL,
  `transaction_type` enum('Lent','Issued') NOT NULL,
  `returned_at` timestamp NULL DEFAULT NULL,
  `asset_condition_at_transaction` int(11) NOT NULL,
  `asset_condition_at_return` int(11) DEFAULT NULL,
  `open_asset_id` int(11) GENERATED ALWAYS AS (case when `returned_at` is null then `asset_id` else NULL end) STORED
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `asset_transactions`
--

INSERT INTO `asset_transactions` (`id`, `batch_id`, `user_id`, `personnel_id`, `asset_id`, `transaction_date`, `remarks`, `transaction_type`, `returned_at`, `asset_condition_at_transaction`, `asset_condition_at_return`) VALUES
(1, 1, 1, 1, 1, '2026-11-03 13:07:23', 'Sample lending transaction', 'Lent', NULL, 2, NULL),
(14, 2, 6, 3, 5, '2026-11-17 14:30:51', NULL, 'Issued', NULL, 2, NULL),
(15, 3, 6, 1, 19, '2026-11-17 15:02:28', NULL, 'Issued', NULL, 4, NULL),
(16, 3, 6, 1, 8, '2026-11-17 15:02:28', NULL, 'Issued', NULL, 2, NULL);

-- --------------------------------------------------------

--
-- Table structure for table `asset_transactions_batches`
--

CREATE TABLE `asset_transactions_batches` (
  `id` int(11) NOT NULL,
  `user_id` int(11) NOT NULL,
  `personnel_id` int(11) NOT NULL,
  `transaction_date` timestamp NOT NULL DEFAULT current_timestamp(),
  `remarks` text DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `asset_transactions_batches`
--

INSERT INTO `asset_transactions_batches` (`id`, `user_id`, `personnel_id`, `transaction_date`, `remarks`) VALUES
(1, 1, 1, '2026-11-03 13:07:23', 'Sample lending transaction'),
(2, 6, 3, '2026-11-17 14:30:51', NULL),
(3, 6, 1, '2026-11-17 15:02:28', 'test issue 2_multiple items');

-- --------------------------------------------------------

--
-- Table structure for table `departments`
--

CREATE TABLE `departments` (
  `id` int(11) NOT NULL,
  `department` varchar(100) NOT NULL,
  `status` varchar(20) NOT NULL DEFAULT 'Active'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `departments`
--

INSERT INTO `departments` (`id`, `department`, `status`) VALUES
(1, 'IT Department - Sample', 'Active'),
(2, 'HR', 'Active'),
(3, 'Registrar', 'Active'),
(4, 'Finance', 'Active'),
(5, 'Admin', 'Active'),
(6, 'Dcs', 'Active'),
(9, 'testing update', 'Archived');

-- --------------------------------------------------------

--
-- Table structure for table `log_activity`
--

CREATE TABLE `log_activity` (
  `log_id` bigint(20) NOT NULL,
  `user_id` int(11) DEFAULT NULL,
  `action` varchar(50) NOT NULL,
  `entity_type` varchar(50) NOT NULL,
  `entity_id` int(11) DEFAULT NULL,
  `description` text DEFAULT NULL,
  `old_value` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_bin DEFAULT NULL CHECK (json_valid(`old_value`)),
  `new_value` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_bin DEFAULT NULL CHECK (json_valid(`new_value`)),
  `status` varchar(10) NOT NULL DEFAULT 'SUCCESS',
  `device_name` varchar(120) DEFAULT NULL,
  `app_version` varchar(20) DEFAULT NULL,
  `created_at` timestamp NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `personnels`
--

CREATE TABLE `personnels` (
  `id` int(11) NOT NULL,
  `first_name` varchar(50) NOT NULL,
  `middle_name` varchar(50) DEFAULT NULL,
  `last_name` varchar(50) NOT NULL,
  `account_status` int(11) NOT NULL,
  `department` int(11) NOT NULL,
  `employee_no` varchar(50) DEFAULT NULL,
  `employee_type` varchar(50) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `personnels`
--

INSERT INTO `personnels` (`id`, `first_name`, `middle_name`, `last_name`, `account_status`, `department`, `employee_no`, `employee_type`) VALUES
(1, 'Juan', NULL, 'Dela Cruz', 1, 1, 'EMP-TEST-001', NULL),
(3, 'v', NULL, 'c', 1, 1, '1010', NULL),
(4, 'm', 'i', 'm', 1, 5, '2010', NULL),
(5, 'a', NULL, 'e', 1, 2, '1228', NULL);

-- --------------------------------------------------------

--
-- Table structure for table `users`
--

CREATE TABLE `users` (
  `id` int(11) NOT NULL,
  `first_name` varchar(50) NOT NULL,
  `middle_name` varchar(50) DEFAULT NULL,
  `last_name` varchar(50) NOT NULL,
  `username` varchar(50) NOT NULL,
  `password_hash` varchar(255) NOT NULL,
  `role` int(11) NOT NULL,
  `account_status` int(11) NOT NULL,
  `created_at` timestamp NOT NULL DEFAULT current_timestamp(),
  `updated_at` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `users`
--

INSERT INTO `users` (`id`, `first_name`, `middle_name`, `last_name`, `username`, `password_hash`, `role`, `account_status`, `created_at`, `updated_at`) VALUES
(1, 'Test', NULL, 'Administrator', 'test_admin', 'TEST_ONLY_NOT_A_REAL_PASSWORD_HASH', 1, 1, '2026-11-02 15:00:51', '2026-11-02 15:00:51'),
(3, 'Ashley Mei', 'Ison', 'Madrid', 'amadrid55', '600000.oliIxV/GuB429KNldO8ygQ==.uyPBS5tFLpQsp/WMtZOrrZRa6iJxHsd4Ee3iMlsUNHY=', 2, 1, '2026-11-13 08:14:32', '2026-11-13 08:14:32'),
(5, 'Angelica Lourdes', NULL, 'Estanislao', 'aestani76', '600000.swBtEVJ/0ne+0a2pYOK4rA==.LMmUmhpt8L+IMS5E9DFPvIfo4/qrufJ/ttLrDWbBqOE=', 1, 1, '2026-11-13 08:24:55', '2026-11-13 08:24:55'),
(6, 'Vich Andrei', 'Tekistekis', 'Cortez', 'vcortez70', '600000.C1JLcRxDLfGcP1wgsDhsjw==.ZTY8tW0jQCX9bpOzI5tLqI9loq1/WhmFocJ3MWhcoxA=', 1, 1, '2026-11-15 08:04:02', '2026-11-15 08:04:02'),
(7, 'Loughshinny', NULL, 'Dublinn', 'ldublin26', '600000.Jm4uVscVjgG4/gVbjxx/RA==.9gVXt/4BMBUcXBgHrHcFxFiwjGeHJ1C4Iyrw0eW0EcA=', 2, 1, '2026-11-15 08:46:48', '2026-11-15 08:46:48'),
(8, 'Angelica', 'B', 'Estanislao', 'aestani50', '600000.7mYqko76pl4e01KzWZadag==.4A62M+owU1fzUzdAPLYuI+Xs2KFuYnk2WCyK1YTb+/g=', 1, 1, '2026-11-18 07:23:14', '2026-11-18 07:23:14');

-- --------------------------------------------------------

--
-- Table structure for table `user_roles`
--

CREATE TABLE `user_roles` (
  `id` int(11) NOT NULL,
  `role` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `user_roles`
--

INSERT INTO `user_roles` (`id`, `role`) VALUES
(1, 'Administrator'),
(2, 'IT Staff');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `account_statuses`
--
ALTER TABLE `account_statuses`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `assets`
--
ALTER TABLE `assets`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `tag` (`tag`),
  ADD UNIQUE KEY `serial_number` (`serial_number`),
  ADD KEY `condition_id` (`condition_id`),
  ADD KEY `idx_assets_status` (`status_id`),
  ADD KEY `idx_assets_category` (`category_id`),
  ADD KEY `fk_assets_parent` (`parent_asset_id`);

--
-- Indexes for table `asset_categories`
--
ALTER TABLE `asset_categories`
  ADD PRIMARY KEY (`id`),
  ADD KEY `parent_category_id` (`parent_category_id`);

--
-- Indexes for table `asset_conditions`
--
ALTER TABLE `asset_conditions`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `asset_statuses`
--
ALTER TABLE `asset_statuses`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `asset_transactions`
--
ALTER TABLE `asset_transactions`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `uq_one_open_assignment` (`open_asset_id`),
  ADD KEY `user_id` (`user_id`),
  ADD KEY `personnel_id` (`personnel_id`),
  ADD KEY `asset_id` (`asset_id`),
  ADD KEY `asset_condition_at_transaction` (`asset_condition_at_transaction`),
  ADD KEY `asset_condition_at_return` (`asset_condition_at_return`),
  ADD KEY `asset_transactions_batch_id_IDX` (`batch_id`) USING BTREE;

--
-- Indexes for table `asset_transactions_batches`
--
ALTER TABLE `asset_transactions_batches`
  ADD PRIMARY KEY (`id`),
  ADD KEY `asset_transactions_batches_users_FK` (`user_id`),
  ADD KEY `asset_transactions_batches_personnels_FK` (`personnel_id`);

--
-- Indexes for table `departments`
--
ALTER TABLE `departments`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `departments_unique` (`department`);

--
-- Indexes for table `log_activity`
--
ALTER TABLE `log_activity`
  ADD PRIMARY KEY (`log_id`),
  ADD KEY `user_id` (`user_id`),
  ADD KEY `idx_log_activity_entity` (`entity_type`,`entity_id`),
  ADD KEY `idx_log_activity_created` (`created_at`);

--
-- Indexes for table `personnels`
--
ALTER TABLE `personnels`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `employee_no` (`employee_no`),
  ADD KEY `account_status` (`account_status`),
  ADD KEY `department` (`department`);

--
-- Indexes for table `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `username` (`username`),
  ADD KEY `role` (`role`),
  ADD KEY `account_status` (`account_status`);

--
-- Indexes for table `user_roles`
--
ALTER TABLE `user_roles`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `account_statuses`
--
ALTER TABLE `account_statuses`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT for table `assets`
--
ALTER TABLE `assets`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=21;

--
-- AUTO_INCREMENT for table `asset_categories`
--
ALTER TABLE `asset_categories`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT for table `asset_conditions`
--
ALTER TABLE `asset_conditions`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT for table `asset_statuses`
--
ALTER TABLE `asset_statuses`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT for table `asset_transactions`
--
ALTER TABLE `asset_transactions`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=17;

--
-- AUTO_INCREMENT for table `asset_transactions_batches`
--
ALTER TABLE `asset_transactions_batches`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT for table `departments`
--
ALTER TABLE `departments`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;

--
-- AUTO_INCREMENT for table `log_activity`
--
ALTER TABLE `log_activity`
  MODIFY `log_id` bigint(20) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `personnels`
--
ALTER TABLE `personnels`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT for table `users`
--
ALTER TABLE `users`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT for table `user_roles`
--
ALTER TABLE `user_roles`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `assets`
--
ALTER TABLE `assets`
  ADD CONSTRAINT `assets_ibfk_1` FOREIGN KEY (`category_id`) REFERENCES `asset_categories` (`id`),
  ADD CONSTRAINT `assets_ibfk_2` FOREIGN KEY (`status_id`) REFERENCES `asset_statuses` (`id`),
  ADD CONSTRAINT `assets_ibfk_3` FOREIGN KEY (`condition_id`) REFERENCES `asset_conditions` (`id`),
  ADD CONSTRAINT `fk_assets_parent` FOREIGN KEY (`parent_asset_id`) REFERENCES `assets` (`id`) ON DELETE SET NULL ON UPDATE CASCADE;

--
-- Constraints for table `asset_categories`
--
ALTER TABLE `asset_categories`
  ADD CONSTRAINT `asset_categories_ibfk_1` FOREIGN KEY (`parent_category_id`) REFERENCES `asset_categories` (`id`) ON DELETE SET NULL ON UPDATE CASCADE;

--
-- Constraints for table `asset_transactions`
--
ALTER TABLE `asset_transactions`
  ADD CONSTRAINT `asset_transactions_asset_transactions_batches_FK` FOREIGN KEY (`batch_id`) REFERENCES `asset_transactions_batches` (`id`),
  ADD CONSTRAINT `asset_transactions_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `users` (`id`),
  ADD CONSTRAINT `asset_transactions_ibfk_2` FOREIGN KEY (`personnel_id`) REFERENCES `personnels` (`id`),
  ADD CONSTRAINT `asset_transactions_ibfk_3` FOREIGN KEY (`asset_id`) REFERENCES `assets` (`id`),
  ADD CONSTRAINT `asset_transactions_ibfk_4` FOREIGN KEY (`asset_condition_at_transaction`) REFERENCES `asset_conditions` (`id`),
  ADD CONSTRAINT `asset_transactions_ibfk_5` FOREIGN KEY (`asset_condition_at_return`) REFERENCES `asset_conditions` (`id`);

--
-- Constraints for table `asset_transactions_batches`
--
ALTER TABLE `asset_transactions_batches`
  ADD CONSTRAINT `asset_transactions_batches_personnels_FK` FOREIGN KEY (`personnel_id`) REFERENCES `personnels` (`id`),
  ADD CONSTRAINT `asset_transactions_batches_users_FK` FOREIGN KEY (`user_id`) REFERENCES `users` (`id`);

--
-- Constraints for table `log_activity`
--
ALTER TABLE `log_activity`
  ADD CONSTRAINT `log_activity_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `users` (`id`) ON DELETE SET NULL ON UPDATE CASCADE;

--
-- Constraints for table `personnels`
--
ALTER TABLE `personnels`
  ADD CONSTRAINT `personnels_ibfk_1` FOREIGN KEY (`account_status`) REFERENCES `account_statuses` (`id`),
  ADD CONSTRAINT `personnels_ibfk_2` FOREIGN KEY (`department`) REFERENCES `departments` (`id`);

--
-- Constraints for table `users`
--
ALTER TABLE `users`
  ADD CONSTRAINT `users_ibfk_1` FOREIGN KEY (`role`) REFERENCES `user_roles` (`id`),
  ADD CONSTRAINT `users_ibfk_2` FOREIGN KEY (`account_status`) REFERENCES `account_statuses` (`id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
