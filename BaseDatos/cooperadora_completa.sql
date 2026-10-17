-- =====================================================================
-- Base de datos COMPLETA de la cooperadora (estructura + datos de prueba)
-- Para importar en una base VACÍA llamada `cooperadora`:
--   phpMyAdmin -> elegir la base -> pestaña Importar -> elegir este archivo -> Continuar
-- Incluye el script original de la base y los cambios necesarios para la app.
-- =====================================================================

-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 13-10-2026 a las 23:54:53
-- Versión del servidor: 10.4.32-MariaDB
-- Versión de PHP: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `cooperadora`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `auditoria`
--

CREATE TABLE `auditoria` (
  `id_auditoria` int(11) NOT NULL,
  `id_usuario` int(11) NOT NULL,
  `entidad` varchar(255) NOT NULL,
  `id_registro` int(11) NOT NULL,
  `accion` varchar(255) NOT NULL,
  `fecha_hora` datetime NOT NULL,
  `datos_previos` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `caja`
--

CREATE TABLE `caja` (
  `id_caja` int(11) NOT NULL,
  `fecha_apertura` datetime NOT NULL,
  `fecha_cierre` datetime DEFAULT NULL,
  `efectivo_inicial` decimal(10,2) NOT NULL,
  `efectivo_ingresos` decimal(10,2) DEFAULT NULL,
  `efectivo_egresos` decimal(10,2) DEFAULT NULL,
  `efectivo_esperado` decimal(10,2) DEFAULT NULL,
  `efectivo_declarado` decimal(10,2) DEFAULT NULL,
  `diferencia_efectivo` decimal(10,2) DEFAULT NULL,
  `estado` enum('abierta','cerrada') NOT NULL DEFAULT 'abierta',
  `abierta_por` int(11) NOT NULL,
  `cerrada_por` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `caja_total_medio_pago`
--

CREATE TABLE `caja_total_medio_pago` (
  `id_caja` int(11) NOT NULL,
  `id_medio_pago` int(11) NOT NULL,
  `total_ingresos` decimal(10,2) NOT NULL DEFAULT 0.00,
  `total_egresos` decimal(10,2) NOT NULL DEFAULT 0.00
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `categoria_egreso`
--

CREATE TABLE `categoria_egreso` (
  `id_categoria_egreso` int(11) NOT NULL,
  `nombre` varchar(255) NOT NULL,
  `descripcion` varchar(255) DEFAULT NULL,
  `activa` tinyint(1) NOT NULL DEFAULT 1,
  `creada_por` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `categoria_ingreso`
--

CREATE TABLE `categoria_ingreso` (
  `id_categoria_ingreso` int(11) NOT NULL,
  `nombre` varchar(255) NOT NULL,
  `descripcion` varchar(255) DEFAULT NULL,
  `activa` tinyint(1) NOT NULL DEFAULT 1,
  `creada_por` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `clave_global`
--

CREATE TABLE `clave_global` (
  `id_clave` int(11) NOT NULL,
  `nombre` varchar(255) NOT NULL,
  `codigo_hash` varchar(255) NOT NULL,
  `accion_protegida` enum('anular_ingreso','anular_egreso','reabrir_caja','modificar_cierre_caja','dar_de_baja_socio','eliminar_usuario') NOT NULL,
  `activa` tinyint(1) NOT NULL DEFAULT 1,
  `creada_en` datetime NOT NULL,
  `actualizada_en` datetime DEFAULT NULL,
  `creada_por` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `cuota`
--

CREATE TABLE `cuota` (
  `id_cuota` int(11) NOT NULL,
  `id_socio` int(11) NOT NULL,
  `periodo` date NOT NULL,
  `importe` decimal(10,2) NOT NULL,
  `fecha_vencimiento` date NOT NULL,
  `estado` enum('pendiente','parcial','pagada','anulada') NOT NULL DEFAULT 'pendiente'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `detalle_ingreso_cuota`
--

CREATE TABLE `detalle_ingreso_cuota` (
  `id_detalle_ingreso_cuota` int(11) NOT NULL,
  `id_ingreso` int(11) NOT NULL,
  `id_cuota` int(11) NOT NULL,
  `monto_aplicado` decimal(10,2) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `egreso`
--

CREATE TABLE `egreso` (
  `id_egreso` int(11) NOT NULL,
  `fecha_hora` datetime NOT NULL,
  `importe` decimal(10,2) NOT NULL,
  `concepto` varchar(255) NOT NULL,
  `nro_comprobante` varchar(255) DEFAULT NULL,
  `archivo_comprobante` varchar(255) DEFAULT NULL,
  `observacion` varchar(255) DEFAULT NULL,
  `estado` enum('registrado','anulado') NOT NULL DEFAULT 'registrado',
  `motivo_anulacion` varchar(255) DEFAULT NULL,
  `fecha_anulacion` datetime DEFAULT NULL,
  `id_medio_pago` int(11) NOT NULL,
  `id_categoria_egreso` int(11) NOT NULL,
  `id_caja` int(11) NOT NULL,
  `registrado_por` int(11) NOT NULL,
  `anulado_por` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `ingreso`
--

CREATE TABLE `ingreso` (
  `id_ingreso` int(11) NOT NULL,
  `fecha_hora` datetime NOT NULL,
  `importe` decimal(10,2) NOT NULL,
  `concepto` varchar(255) NOT NULL,
  `nro_comprobante` varchar(255) DEFAULT NULL,
  `archivo_comprobante` varchar(255) DEFAULT NULL,
  `observacion` varchar(255) DEFAULT NULL,
  `estado` enum('registrado','anulado') NOT NULL DEFAULT 'registrado',
  `motivo_anulacion` varchar(255) DEFAULT NULL,
  `fecha_anulacion` datetime DEFAULT NULL,
  `id_socio` int(11) DEFAULT NULL,
  `id_medio_pago` int(11) NOT NULL,
  `id_categoria_ingreso` int(11) NOT NULL,
  `id_caja` int(11) NOT NULL,
  `registrado_por` int(11) NOT NULL,
  `anulado_por` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `medio_pago`
--

CREATE TABLE `medio_pago` (
  `id_medio_pago` int(11) NOT NULL,
  `nombre` varchar(255) NOT NULL,
  `activo` tinyint(1) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `medio_pago`
--

INSERT INTO `medio_pago` (`id_medio_pago`, `nombre`, `activo`) VALUES
(1, 'Efectivo', 1),
(2, 'Transferencia', 1),
(3, 'Tarjeta de debito', 1),
(4, 'Tarjeta de credito', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `rol`
--

CREATE TABLE `rol` (
  `id_rol` int(11) NOT NULL,
  `nombre` enum('operador','contador','administrador') NOT NULL,
  `descripcion` varchar(255) DEFAULT NULL,
  `activo` tinyint(1) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `rol`
--

INSERT INTO `rol` (`id_rol`, `nombre`, `descripcion`, `activo`) VALUES
(1, 'operador', 'Gestiona ingresos y operaciones de caja', 1),
(2, 'contador', 'Gestiona egresos y control contable', 1),
(3, 'administrador', 'Administra usuarios y configuracion del sistema', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `socio`
--

CREATE TABLE `socio` (
  `id_socio` int(11) NOT NULL,
  `dni` varchar(255) NOT NULL,
  `nombre` varchar(255) NOT NULL,
  `apellido` varchar(255) NOT NULL,
  `telefono` varchar(255) DEFAULT NULL,
  `email` varchar(255) DEFAULT NULL,
  `tipo` enum('socio','responsable') NOT NULL,
  `estado` enum('activo','inactivo','baja') NOT NULL DEFAULT 'activo',
  `fecha_alta` date NOT NULL,
  `fecha_baja` date DEFAULT NULL,
  `dado_de_alta_por` int(11) NOT NULL,
  `dado_de_baja_por` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuario`
--

CREATE TABLE `usuario` (
  `id_usuario` int(11) NOT NULL,
  `nombre` varchar(255) NOT NULL,
  `apellido` varchar(255) NOT NULL,
  `email` varchar(255) NOT NULL,
  `password_hash` varchar(255) NOT NULL,
  `activo` tinyint(1) NOT NULL DEFAULT 1,
  `creado_en` datetime NOT NULL,
  `actualizado_en` datetime DEFAULT NULL,
  `ultimo_acceso` datetime DEFAULT NULL,
  `id_rol` int(11) NOT NULL,
  `creado_por` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `auditoria`
--
ALTER TABLE `auditoria`
  ADD PRIMARY KEY (`id_auditoria`),
  ADD KEY `idx_auditoria_entidad_registro` (`entidad`,`id_registro`),
  ADD KEY `idx_auditoria_fecha` (`fecha_hora`),
  ADD KEY `idx_auditoria_usuario` (`id_usuario`);

--
-- Indices de la tabla `caja`
--
ALTER TABLE `caja`
  ADD PRIMARY KEY (`id_caja`),
  ADD KEY `abierta_por` (`abierta_por`),
  ADD KEY `cerrada_por` (`cerrada_por`),
  ADD KEY `idx_caja_estado` (`estado`),
  ADD KEY `idx_caja_fecha_apertura` (`fecha_apertura`);

--
-- Indices de la tabla `caja_total_medio_pago`
--
ALTER TABLE `caja_total_medio_pago`
  ADD PRIMARY KEY (`id_caja`,`id_medio_pago`),
  ADD KEY `id_medio_pago` (`id_medio_pago`);

--
-- Indices de la tabla `categoria_egreso`
--
ALTER TABLE `categoria_egreso`
  ADD PRIMARY KEY (`id_categoria_egreso`),
  ADD UNIQUE KEY `nombre` (`nombre`),
  ADD KEY `creada_por` (`creada_por`);

--
-- Indices de la tabla `categoria_ingreso`
--
ALTER TABLE `categoria_ingreso`
  ADD PRIMARY KEY (`id_categoria_ingreso`),
  ADD UNIQUE KEY `nombre` (`nombre`),
  ADD KEY `creada_por` (`creada_por`);

--
-- Indices de la tabla `clave_global`
--
ALTER TABLE `clave_global`
  ADD PRIMARY KEY (`id_clave`),
  ADD KEY `creada_por` (`creada_por`);

--
-- Indices de la tabla `cuota`
--
ALTER TABLE `cuota`
  ADD PRIMARY KEY (`id_cuota`),
  ADD UNIQUE KEY `id_socio` (`id_socio`,`periodo`),
  ADD KEY `idx_cuota_vencimiento` (`fecha_vencimiento`),
  ADD KEY `idx_cuota_estado` (`estado`);

--
-- Indices de la tabla `detalle_ingreso_cuota`
--
ALTER TABLE `detalle_ingreso_cuota`
  ADD PRIMARY KEY (`id_detalle_ingreso_cuota`),
  ADD UNIQUE KEY `id_ingreso` (`id_ingreso`,`id_cuota`),
  ADD KEY `idx_detalle_cuota` (`id_cuota`);

--
-- Indices de la tabla `egreso`
--
ALTER TABLE `egreso`
  ADD PRIMARY KEY (`id_egreso`),
  ADD KEY `id_medio_pago` (`id_medio_pago`),
  ADD KEY `registrado_por` (`registrado_por`),
  ADD KEY `anulado_por` (`anulado_por`),
  ADD KEY `idx_egreso_fecha` (`fecha_hora`),
  ADD KEY `idx_egreso_caja` (`id_caja`),
  ADD KEY `idx_egreso_categoria` (`id_categoria_egreso`),
  ADD KEY `idx_egreso_estado` (`estado`);

--
-- Indices de la tabla `ingreso`
--
ALTER TABLE `ingreso`
  ADD PRIMARY KEY (`id_ingreso`),
  ADD KEY `id_medio_pago` (`id_medio_pago`),
  ADD KEY `registrado_por` (`registrado_por`),
  ADD KEY `anulado_por` (`anulado_por`),
  ADD KEY `idx_ingreso_fecha` (`fecha_hora`),
  ADD KEY `idx_ingreso_caja` (`id_caja`),
  ADD KEY `idx_ingreso_socio` (`id_socio`),
  ADD KEY `idx_ingreso_categoria` (`id_categoria_ingreso`),
  ADD KEY `idx_ingreso_estado` (`estado`);

--
-- Indices de la tabla `medio_pago`
--
ALTER TABLE `medio_pago`
  ADD PRIMARY KEY (`id_medio_pago`),
  ADD UNIQUE KEY `nombre` (`nombre`);

--
-- Indices de la tabla `rol`
--
ALTER TABLE `rol`
  ADD PRIMARY KEY (`id_rol`),
  ADD UNIQUE KEY `nombre` (`nombre`);

--
-- Indices de la tabla `socio`
--
ALTER TABLE `socio`
  ADD PRIMARY KEY (`id_socio`),
  ADD UNIQUE KEY `dni` (`dni`),
  ADD KEY `dado_de_alta_por` (`dado_de_alta_por`),
  ADD KEY `dado_de_baja_por` (`dado_de_baja_por`),
  ADD KEY `idx_socio_apellido` (`apellido`),
  ADD KEY `idx_socio_tipo` (`tipo`),
  ADD KEY `idx_socio_estado` (`estado`);

--
-- Indices de la tabla `usuario`
--
ALTER TABLE `usuario`
  ADD PRIMARY KEY (`id_usuario`),
  ADD UNIQUE KEY `email` (`email`),
  ADD KEY `id_rol` (`id_rol`),
  ADD KEY `creado_por` (`creado_por`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `auditoria`
--
ALTER TABLE `auditoria`
  MODIFY `id_auditoria` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `caja`
--
ALTER TABLE `caja`
  MODIFY `id_caja` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `categoria_egreso`
--
ALTER TABLE `categoria_egreso`
  MODIFY `id_categoria_egreso` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `categoria_ingreso`
--
ALTER TABLE `categoria_ingreso`
  MODIFY `id_categoria_ingreso` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `clave_global`
--
ALTER TABLE `clave_global`
  MODIFY `id_clave` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `cuota`
--
ALTER TABLE `cuota`
  MODIFY `id_cuota` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `detalle_ingreso_cuota`
--
ALTER TABLE `detalle_ingreso_cuota`
  MODIFY `id_detalle_ingreso_cuota` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `egreso`
--
ALTER TABLE `egreso`
  MODIFY `id_egreso` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `ingreso`
--
ALTER TABLE `ingreso`
  MODIFY `id_ingreso` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `medio_pago`
--
ALTER TABLE `medio_pago`
  MODIFY `id_medio_pago` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT de la tabla `rol`
--
ALTER TABLE `rol`
  MODIFY `id_rol` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT de la tabla `socio`
--
ALTER TABLE `socio`
  MODIFY `id_socio` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `usuario`
--
ALTER TABLE `usuario`
  MODIFY `id_usuario` int(11) NOT NULL AUTO_INCREMENT;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `auditoria`
--
ALTER TABLE `auditoria`
  ADD CONSTRAINT `auditoria_ibfk_1` FOREIGN KEY (`id_usuario`) REFERENCES `usuario` (`id_usuario`);

--
-- Filtros para la tabla `caja`
--
ALTER TABLE `caja`
  ADD CONSTRAINT `caja_ibfk_1` FOREIGN KEY (`abierta_por`) REFERENCES `usuario` (`id_usuario`),
  ADD CONSTRAINT `caja_ibfk_2` FOREIGN KEY (`cerrada_por`) REFERENCES `usuario` (`id_usuario`);

--
-- Filtros para la tabla `caja_total_medio_pago`
--
ALTER TABLE `caja_total_medio_pago`
  ADD CONSTRAINT `caja_total_medio_pago_ibfk_1` FOREIGN KEY (`id_caja`) REFERENCES `caja` (`id_caja`),
  ADD CONSTRAINT `caja_total_medio_pago_ibfk_2` FOREIGN KEY (`id_medio_pago`) REFERENCES `medio_pago` (`id_medio_pago`);

--
-- Filtros para la tabla `categoria_egreso`
--
ALTER TABLE `categoria_egreso`
  ADD CONSTRAINT `categoria_egreso_ibfk_1` FOREIGN KEY (`creada_por`) REFERENCES `usuario` (`id_usuario`);

--
-- Filtros para la tabla `categoria_ingreso`
--
ALTER TABLE `categoria_ingreso`
  ADD CONSTRAINT `categoria_ingreso_ibfk_1` FOREIGN KEY (`creada_por`) REFERENCES `usuario` (`id_usuario`);

--
-- Filtros para la tabla `clave_global`
--
ALTER TABLE `clave_global`
  ADD CONSTRAINT `clave_global_ibfk_1` FOREIGN KEY (`creada_por`) REFERENCES `usuario` (`id_usuario`);

--
-- Filtros para la tabla `cuota`
--
ALTER TABLE `cuota`
  ADD CONSTRAINT `cuota_ibfk_1` FOREIGN KEY (`id_socio`) REFERENCES `socio` (`id_socio`);

--
-- Filtros para la tabla `detalle_ingreso_cuota`
--
ALTER TABLE `detalle_ingreso_cuota`
  ADD CONSTRAINT `detalle_ingreso_cuota_ibfk_1` FOREIGN KEY (`id_ingreso`) REFERENCES `ingreso` (`id_ingreso`),
  ADD CONSTRAINT `detalle_ingreso_cuota_ibfk_2` FOREIGN KEY (`id_cuota`) REFERENCES `cuota` (`id_cuota`);

--
-- Filtros para la tabla `egreso`
--
ALTER TABLE `egreso`
  ADD CONSTRAINT `egreso_ibfk_1` FOREIGN KEY (`id_medio_pago`) REFERENCES `medio_pago` (`id_medio_pago`),
  ADD CONSTRAINT `egreso_ibfk_2` FOREIGN KEY (`id_categoria_egreso`) REFERENCES `categoria_egreso` (`id_categoria_egreso`),
  ADD CONSTRAINT `egreso_ibfk_3` FOREIGN KEY (`id_caja`) REFERENCES `caja` (`id_caja`),
  ADD CONSTRAINT `egreso_ibfk_4` FOREIGN KEY (`registrado_por`) REFERENCES `usuario` (`id_usuario`),
  ADD CONSTRAINT `egreso_ibfk_5` FOREIGN KEY (`anulado_por`) REFERENCES `usuario` (`id_usuario`);

--
-- Filtros para la tabla `ingreso`
--
ALTER TABLE `ingreso`
  ADD CONSTRAINT `ingreso_ibfk_1` FOREIGN KEY (`id_socio`) REFERENCES `socio` (`id_socio`),
  ADD CONSTRAINT `ingreso_ibfk_2` FOREIGN KEY (`id_medio_pago`) REFERENCES `medio_pago` (`id_medio_pago`),
  ADD CONSTRAINT `ingreso_ibfk_3` FOREIGN KEY (`id_categoria_ingreso`) REFERENCES `categoria_ingreso` (`id_categoria_ingreso`),
  ADD CONSTRAINT `ingreso_ibfk_4` FOREIGN KEY (`id_caja`) REFERENCES `caja` (`id_caja`),
  ADD CONSTRAINT `ingreso_ibfk_5` FOREIGN KEY (`registrado_por`) REFERENCES `usuario` (`id_usuario`),
  ADD CONSTRAINT `ingreso_ibfk_6` FOREIGN KEY (`anulado_por`) REFERENCES `usuario` (`id_usuario`);

--
-- Filtros para la tabla `socio`
--
ALTER TABLE `socio`
  ADD CONSTRAINT `socio_ibfk_1` FOREIGN KEY (`dado_de_alta_por`) REFERENCES `usuario` (`id_usuario`),
  ADD CONSTRAINT `socio_ibfk_2` FOREIGN KEY (`dado_de_baja_por`) REFERENCES `usuario` (`id_usuario`);

--
-- Filtros para la tabla `usuario`
--
ALTER TABLE `usuario`
  ADD CONSTRAINT `usuario_ibfk_1` FOREIGN KEY (`id_rol`) REFERENCES `rol` (`id_rol`),
  ADD CONSTRAINT `usuario_ibfk_2` FOREIGN KEY (`creado_por`) REFERENCES `usuario` (`id_usuario`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;

-- =====================================================================
-- Cambios para la app + datos de prueba
-- =====================================================================
-- 1) Cambios en la estructura -----------------------------------------

-- La app inicia sesión con un nombre de usuario (el email pasa a ser opcional).
ALTER TABLE `usuario`
  ADD COLUMN `nombre_usuario` varchar(30) DEFAULT NULL AFTER `id_usuario`,
  ADD UNIQUE KEY `uq_usuario_nombre_usuario` (`nombre_usuario`),
  MODIFY `email` varchar(255) DEFAULT NULL;

-- Un socio puede tener un responsable (otro socio de tipo 'responsable').
ALTER TABLE `socio`
  ADD COLUMN `id_responsable` int(11) DEFAULT NULL AFTER `tipo`,
  ADD KEY `idx_socio_responsable` (`id_responsable`),
  ADD CONSTRAINT `socio_ibfk_3` FOREIGN KEY (`id_responsable`) REFERENCES `socio` (`id_socio`);

-- 2) Datos de prueba ---------------------------------------------------
-- Usuarios (contraseñas: admin123 / operador123 / contador123), guardadas con el mismo hash PBKDF2 que usa la app.
INSERT INTO `usuario` (`nombre_usuario`, `nombre`, `apellido`, `password_hash`, `activo`, `creado_en`, `id_rol`) VALUES
('admin',    'Ana',   'Admin',     '100000.rmo3Fp0Ab6VFkkU9U3R+9Q==.rMoY8Kh1Zk5gIdfrzZBxt0+PzmLKZq9udHDRL8lIFj4=',    1, NOW(), (SELECT `id_rol` FROM `rol` WHERE `nombre` = 'administrador')),
('operador', 'Omar',  'Operador',  '100000.V+TUoHdBPEZHp70kL1AZ5g==.J7y0QfOq7NuwDR62pJkP7dm8qQ/hhXzLZTTgf/n1bag=', 1, NOW(), (SELECT `id_rol` FROM `rol` WHERE `nombre` = 'operador')),
('contador', 'Carla', 'Contadora', '100000.LSyYBdE4apxZr27neaFsAg==.PcrRwj3/1uJt1a+yil7YQdflzdib+JjJcCLbRBbdzwc=', 1, NOW(), (SELECT `id_rol` FROM `rol` WHERE `nombre` = 'contador'));

SET @admin := (SELECT `id_usuario` FROM `usuario` WHERE `nombre_usuario` = 'admin');

-- Categorías de ingreso: los nombres deben coincidir con el enum CategoriaIngreso del código.
INSERT INTO `categoria_ingreso` (`nombre`, `descripcion`, `activa`, `creada_por`) VALUES
('CuotaSocial', 'Pago de cuota social', 1, @admin),
('Donacion',    'Donación recibida',    1, @admin),
('Subsidio',    'Subsidio recibido',    1, @admin);

-- Categorías de egreso (en el código son los "motivos de egreso").
INSERT INTO `categoria_egreso` (`nombre`, `descripcion`, `activa`, `creada_por`) VALUES
('Materiales',    NULL, 1, @admin),
('Servicios',     NULL, 1, @admin),
('Mantenimiento', NULL, 1, @admin),
('Eventos',       NULL, 1, @admin),
('Otro',          NULL, 1, @admin);

-- Socios (Martín es responsable de Sofía) y sus cuotas.
INSERT INTO `socio` (`dni`, `nombre`, `apellido`, `telefono`, `email`, `tipo`, `estado`, `fecha_alta`, `dado_de_alta_por`) VALUES
('30123456', 'Lucía',  'Fernández', '1155550101', 'lucia@mail.com',  'socio',       'activo', CURDATE(), @admin),
('28456789', 'Martín', 'Gómez',     '1155550202', 'martin@mail.com', 'responsable', 'activo', CURDATE(), @admin),
('40111222', 'Sofía',  'Pérez',     NULL,         NULL,              'socio',       'activo', CURDATE(), @admin);

SET @lucia  := (SELECT `id_socio` FROM `socio` WHERE `dni` = '30123456');
SET @martin := (SELECT `id_socio` FROM `socio` WHERE `dni` = '28456789');
UPDATE `socio` SET `id_responsable` = @martin WHERE `dni` = '40111222';

INSERT INTO `cuota` (`id_socio`, `periodo`, `importe`, `fecha_vencimiento`, `estado`) VALUES
(@lucia,  '2025-01-01', 5000.00, '2025-12-31', 'pendiente'),
(@lucia,  '2026-01-01', 5000.00, '2026-12-31', 'pendiente'),
(@martin, '2026-01-01', 5000.00, '2026-12-31', 'pendiente');
