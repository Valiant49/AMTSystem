-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Nov 04, 2026 at 11:36 AM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `AMTSystem`
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
  `components` text DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `assets`
--

INSERT INTO `assets` (`id`, `name`, `tag`, `category_id`, `parent_asset_id`, `status_id`, `condition_id`, `description`, `serial_number`, `components`) VALUES
(1, 'System Unit', 'AST-001', 2, NULL, 1, 2, 'Main computer system unit', 'SN-001', NULL),
(2, 'Motherboard', 'AST-002', 2, NULL, 1, 2, 'System unit motherboard', NULL, NULL),
(3, 'RAM', 'AST-003', 2, NULL, 1, 2, 'System unit memory', NULL, NULL),
(4, 'Storage', 'AST-004', 2, NULL, 1, 2, 'System unit storage', NULL, NULL);

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

INSERT INTO `asset_transactions` (`id`, `user_id`, `personnel_id`, `asset_id`, `transaction_date`, `remarks`, `transaction_type`, `returned_at`, `asset_condition_at_transaction`, `asset_condition_at_return`) VALUES
(1, 1, 1, 1, '2026-11-03 13:07:23', 'Sample lending transaction', 'Lent', NULL, 2, NULL);

-- --------------------------------------------------------

--
-- Table structure for table `departments`
--

CREATE TABLE `departments` (
  `id` int(11) NOT NULL,
  `department` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `departments`
--

INSERT INTO `departments` (`id`, `department`) VALUES
(1, 'IT Department - Sample');

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
  `employee_no` varchar(50) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `personnels`
--

INSERT INTO `personnels` (`id`, `first_name`, `middle_name`, `last_name`, `account_status`, `department`, `employee_no`) VALUES
(1, 'Juan', NULL, 'Dela Cruz', 1, 1, 'EMP-TEST-001');

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
(1, 'Test', NULL, 'Administrator', 'test_admin', 'TEST_ONLY_NOT_A_REAL_PASSWORD_HASH', 1, 1, '2026-11-02 15:00:51', '2026-11-02 15:00:51');

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
  ADD KEY `asset_condition_at_return` (`asset_condition_at_return`);

--
-- Indexes for table `departments`
--
ALTER TABLE `departments`
  ADD PRIMARY KEY (`id`);

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
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

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
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `departments`
--
ALTER TABLE `departments`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `log_activity`
--
ALTER TABLE `log_activity`
  MODIFY `log_id` bigint(20) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `personnels`
--
ALTER TABLE `personnels`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `users`
--
ALTER TABLE `users`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

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
  ADD CONSTRAINT `asset_transactions_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `users` (`id`),
  ADD CONSTRAINT `asset_transactions_ibfk_2` FOREIGN KEY (`personnel_id`) REFERENCES `personnels` (`id`),
  ADD CONSTRAINT `asset_transactions_ibfk_3` FOREIGN KEY (`asset_id`) REFERENCES `assets` (`id`),
  ADD CONSTRAINT `asset_transactions_ibfk_4` FOREIGN KEY (`asset_condition_at_transaction`) REFERENCES `asset_conditions` (`id`),
  ADD CONSTRAINT `asset_transactions_ibfk_5` FOREIGN KEY (`asset_condition_at_return`) REFERENCES `asset_conditions` (`id`);

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
