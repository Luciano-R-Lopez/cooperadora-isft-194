-- =====================================================================
-- Base de datos de la Cooperadora ISFT 194 (estructura + datos iniciales)
--
-- Es el script de la rama feature/mysql-agus adaptado a la aplicación actual (rama main):
--   * un usuario puede tener varios roles (tabla usuario_rol);
--   * cada egreso tiene, además del motivo, un grupo: Estudiantes, Administración u Otros
--     (tabla grupo_egreso);
--   * los largos máximos de los textos son los mismos que valida la aplicación.
-- Las equivalencias entre tablas y clases están en BaseDeDatos/README.md.
--
-- Todavía NO está conectada: la aplicación sigue guardando los datos en memoria.
--
-- Para importar en una base VACÍA (por ejemplo `cooperadora`):
--   phpMyAdmin -> elegir la base -> pestaña Importar -> elegir este archivo -> Continuar
-- Probado con MariaDB 10.4 (XAMPP).
-- =====================================================================

SET NAMES utf8mb4;
SET time_zone = '-03:00';
START TRANSACTION;

-- ---------------------------------------------------------------------
-- Usuarios y roles
-- ---------------------------------------------------------------------

CREATE TABLE `rol` (
  `id_rol` int(11) NOT NULL AUTO_INCREMENT,
  `nombre` enum('administrador','operador','contador') NOT NULL,
  `descripcion` varchar(255) DEFAULT NULL,
  `activo` tinyint(1) NOT NULL DEFAULT 1,
  PRIMARY KEY (`id_rol`),
  UNIQUE KEY `uq_rol_nombre` (`nombre`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Se inicia sesión con nombre_usuario; el email es opcional.
-- password_hash: "iteraciones.salt.hash" (PBKDF2-SHA256), el formato de PasswordHasher.
CREATE TABLE `usuario` (
  `id_usuario` int(11) NOT NULL AUTO_INCREMENT,
  `nombre_usuario` varchar(30) NOT NULL,
  `nombre` varchar(50) NOT NULL,
  `apellido` varchar(50) NOT NULL,
  `email` varchar(255) DEFAULT NULL,
  `password_hash` varchar(255) NOT NULL,
  `activo` tinyint(1) NOT NULL DEFAULT 1,
  `creado_en` datetime NOT NULL,
  `actualizado_en` datetime DEFAULT NULL,
  `ultimo_acceso` datetime DEFAULT NULL,
  `creado_por` int(11) DEFAULT NULL,
  PRIMARY KEY (`id_usuario`),
  UNIQUE KEY `uq_usuario_nombre_usuario` (`nombre_usuario`),
  UNIQUE KEY `uq_usuario_email` (`email`),
  KEY `idx_usuario_creado_por` (`creado_por`),
  CONSTRAINT `fk_usuario_creado_por` FOREIGN KEY (`creado_por`) REFERENCES `usuario` (`id_usuario`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Nuevo: reemplaza a la columna usuario.id_rol, porque un usuario puede tener varios roles
-- (por ejemplo, operador y contador a la vez). La aplicación exige al menos uno.
CREATE TABLE `usuario_rol` (
  `id_usuario` int(11) NOT NULL,
  `id_rol` int(11) NOT NULL,
  PRIMARY KEY (`id_usuario`, `id_rol`),
  KEY `idx_usuario_rol_rol` (`id_rol`),
  CONSTRAINT `fk_usuario_rol_usuario` FOREIGN KEY (`id_usuario`) REFERENCES `usuario` (`id_usuario`) ON DELETE CASCADE,
  CONSTRAINT `fk_usuario_rol_rol` FOREIGN KEY (`id_rol`) REFERENCES `rol` (`id_rol`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Del diseño original; la aplicación todavía no la usa.
CREATE TABLE `clave_global` (
  `id_clave` int(11) NOT NULL AUTO_INCREMENT,
  `nombre` varchar(255) NOT NULL,
  `codigo_hash` varchar(255) NOT NULL,
  `accion_protegida` enum('anular_ingreso','anular_egreso','reabrir_caja','modificar_cierre_caja','dar_de_baja_socio','eliminar_usuario') NOT NULL,
  `activa` tinyint(1) NOT NULL DEFAULT 1,
  `creada_en` datetime NOT NULL,
  `actualizada_en` datetime DEFAULT NULL,
  `creada_por` int(11) NOT NULL,
  PRIMARY KEY (`id_clave`),
  KEY `idx_clave_global_creada_por` (`creada_por`),
  CONSTRAINT `fk_clave_global_creada_por` FOREIGN KEY (`creada_por`) REFERENCES `usuario` (`id_usuario`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- ---------------------------------------------------------------------
-- Catálogos que el administrador mantiene desde la aplicación
-- (los inactivos no se ofrecen para movimientos nuevos)
-- ---------------------------------------------------------------------

CREATE TABLE `medio_pago` (
  `id_medio_pago` int(11) NOT NULL AUTO_INCREMENT,
  `nombre` varchar(50) NOT NULL,
  `activo` tinyint(1) NOT NULL DEFAULT 1,
  PRIMARY KEY (`id_medio_pago`),
  UNIQUE KEY `uq_medio_pago_nombre` (`nombre`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- En la aplicación es el enum CategoriaIngreso: los nombres tienen que coincidir con el código.
CREATE TABLE `categoria_ingreso` (
  `id_categoria_ingreso` int(11) NOT NULL AUTO_INCREMENT,
  `nombre` varchar(50) NOT NULL,
  `descripcion` varchar(255) DEFAULT NULL,
  `activa` tinyint(1) NOT NULL DEFAULT 1,
  `creada_por` int(11) NOT NULL,
  PRIMARY KEY (`id_categoria_ingreso`),
  UNIQUE KEY `uq_categoria_ingreso_nombre` (`nombre`),
  KEY `idx_categoria_ingreso_creada_por` (`creada_por`),
  CONSTRAINT `fk_categoria_ingreso_creada_por` FOREIGN KEY (`creada_por`) REFERENCES `usuario` (`id_usuario`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- En la aplicación son los "motivos de egreso" (clase MotivoEgreso).
CREATE TABLE `categoria_egreso` (
  `id_categoria_egreso` int(11) NOT NULL AUTO_INCREMENT,
  `nombre` varchar(50) NOT NULL,
  `descripcion` varchar(255) DEFAULT NULL,
  `activa` tinyint(1) NOT NULL DEFAULT 1,
  `creada_por` int(11) NOT NULL,
  PRIMARY KEY (`id_categoria_egreso`),
  UNIQUE KEY `uq_categoria_egreso_nombre` (`nombre`),
  KEY `idx_categoria_egreso_creada_por` (`creada_por`),
  CONSTRAINT `fk_categoria_egreso_creada_por` FOREIGN KEY (`creada_por`) REFERENCES `usuario` (`id_usuario`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Nuevo: de quién es el gasto (Estudiantes, Administración, Otros). Es independiente del motivo:
-- un arreglo de computadora puede ser de los estudiantes o de la administración.
CREATE TABLE `grupo_egreso` (
  `id_grupo_egreso` int(11) NOT NULL AUTO_INCREMENT,
  `nombre` varchar(50) NOT NULL,
  `activo` tinyint(1) NOT NULL DEFAULT 1,
  `creado_por` int(11) NOT NULL,
  PRIMARY KEY (`id_grupo_egreso`),
  UNIQUE KEY `uq_grupo_egreso_nombre` (`nombre`),
  KEY `idx_grupo_egreso_creado_por` (`creado_por`),
  CONSTRAINT `fk_grupo_egreso_creado_por` FOREIGN KEY (`creado_por`) REFERENCES `usuario` (`id_usuario`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- ---------------------------------------------------------------------
-- Socios y cuotas
-- ---------------------------------------------------------------------

-- La baja es lógica: el socio queda con estado 'baja' y conserva sus cuotas y pagos.
-- id_responsable: otro socio de tipo 'responsable' que paga por este (opcional).
CREATE TABLE `socio` (
  `id_socio` int(11) NOT NULL AUTO_INCREMENT,
  `dni` varchar(8) NOT NULL,
  `nombre` varchar(50) NOT NULL,
  `apellido` varchar(50) NOT NULL,
  `telefono` varchar(20) DEFAULT NULL,
  `email` varchar(255) DEFAULT NULL,
  `tipo` enum('socio','responsable') NOT NULL,
  `id_responsable` int(11) DEFAULT NULL,
  `estado` enum('activo','inactivo','baja') NOT NULL DEFAULT 'activo',
  `fecha_alta` date NOT NULL,
  `fecha_baja` date DEFAULT NULL,
  `dado_de_alta_por` int(11) NOT NULL,
  `dado_de_baja_por` int(11) DEFAULT NULL,
  PRIMARY KEY (`id_socio`),
  UNIQUE KEY `uq_socio_dni` (`dni`),
  KEY `idx_socio_responsable` (`id_responsable`),
  KEY `idx_socio_alta_por` (`dado_de_alta_por`),
  KEY `idx_socio_baja_por` (`dado_de_baja_por`),
  CONSTRAINT `fk_socio_responsable` FOREIGN KEY (`id_responsable`) REFERENCES `socio` (`id_socio`),
  CONSTRAINT `fk_socio_alta_por` FOREIGN KEY (`dado_de_alta_por`) REFERENCES `usuario` (`id_usuario`),
  CONSTRAINT `fk_socio_baja_por` FOREIGN KEY (`dado_de_baja_por`) REFERENCES `usuario` (`id_usuario`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Valor de la cuota social de cada año (el que se propone al generar las cuotas).
CREATE TABLE `valor_cuota` (
  `id_valor_cuota` int(11) NOT NULL AUTO_INCREMENT,
  `anio` int(11) NOT NULL,
  `monto` decimal(10,2) NOT NULL,
  PRIMARY KEY (`id_valor_cuota`),
  UNIQUE KEY `uq_valor_cuota_anio` (`anio`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- En la aplicación la cuota es anual (solo tiene año): se guarda como período 1/1 y vencimiento 31/12.
-- Una cuota por socio y año. La aplicación usa solo los estados 'pendiente' y 'pagada'.
CREATE TABLE `cuota` (
  `id_cuota` int(11) NOT NULL AUTO_INCREMENT,
  `id_socio` int(11) NOT NULL,
  `periodo` date NOT NULL,
  `importe` decimal(10,2) NOT NULL,
  `fecha_vencimiento` date NOT NULL,
  `estado` enum('pendiente','parcial','pagada','anulada') NOT NULL DEFAULT 'pendiente',
  PRIMARY KEY (`id_cuota`),
  UNIQUE KEY `uq_cuota_socio_periodo` (`id_socio`, `periodo`),
  KEY `idx_cuota_vencimiento` (`fecha_vencimiento`),
  KEY `idx_cuota_estado` (`estado`),
  CONSTRAINT `fk_cuota_socio` FOREIGN KEY (`id_socio`) REFERENCES `socio` (`id_socio`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- ---------------------------------------------------------------------
-- Caja y movimientos
-- ---------------------------------------------------------------------

-- Con la caja abierta, efectivo_esperado es el MontoTotal (inicial + ingresos - egresos).
-- Al cerrarla, efectivo_declarado es lo que se contó y diferencia_efectivo = declarado - esperado.
CREATE TABLE `caja` (
  `id_caja` int(11) NOT NULL AUTO_INCREMENT,
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
  `cerrada_por` int(11) DEFAULT NULL,
  PRIMARY KEY (`id_caja`),
  KEY `idx_caja_estado` (`estado`),
  KEY `idx_caja_fecha_apertura` (`fecha_apertura`),
  KEY `idx_caja_abierta_por` (`abierta_por`),
  KEY `idx_caja_cerrada_por` (`cerrada_por`),
  CONSTRAINT `fk_caja_abierta_por` FOREIGN KEY (`abierta_por`) REFERENCES `usuario` (`id_usuario`),
  CONSTRAINT `fk_caja_cerrada_por` FOREIGN KEY (`cerrada_por`) REFERENCES `usuario` (`id_usuario`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Total de una caja por medio de pago. La clave es caja + medio (no tiene id propio);
-- el Monto de la aplicación es total_ingresos - total_egresos.
CREATE TABLE `caja_total_medio_pago` (
  `id_caja` int(11) NOT NULL,
  `id_medio_pago` int(11) NOT NULL,
  `total_ingresos` decimal(10,2) NOT NULL DEFAULT 0.00,
  `total_egresos` decimal(10,2) NOT NULL DEFAULT 0.00,
  PRIMARY KEY (`id_caja`, `id_medio_pago`),
  KEY `idx_caja_total_medio_pago` (`id_medio_pago`),
  CONSTRAINT `fk_caja_total_caja` FOREIGN KEY (`id_caja`) REFERENCES `caja` (`id_caja`),
  CONSTRAINT `fk_caja_total_medio_pago` FOREIGN KEY (`id_medio_pago`) REFERENCES `medio_pago` (`id_medio_pago`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- La aplicación guarda ingresos y egresos juntos (clase Movimiento) y los identifica por id.
-- Para que ningún id se repita, los egresos numeran desde 1.000.001 (ver el AUTO_INCREMENT de egreso).
-- concepto: la aplicación no lo pide; se guarda el texto de la categoría o del motivo.
CREATE TABLE `ingreso` (
  `id_ingreso` int(11) NOT NULL AUTO_INCREMENT,
  `fecha_hora` datetime NOT NULL,
  `importe` decimal(10,2) NOT NULL,
  `concepto` varchar(255) NOT NULL,
  `nro_comprobante` varchar(255) DEFAULT NULL,
  `archivo_comprobante` varchar(255) DEFAULT NULL,
  `observacion` varchar(255) DEFAULT NULL,
  `estado` enum('registrado','anulado') NOT NULL DEFAULT 'registrado',
  `motivo_anulacion` varchar(200) DEFAULT NULL,
  `fecha_anulacion` datetime DEFAULT NULL,
  `id_socio` int(11) DEFAULT NULL,
  `id_medio_pago` int(11) NOT NULL,
  `id_categoria_ingreso` int(11) NOT NULL,
  `id_caja` int(11) NOT NULL,
  `registrado_por` int(11) NOT NULL,
  `anulado_por` int(11) DEFAULT NULL,
  PRIMARY KEY (`id_ingreso`),
  KEY `idx_ingreso_fecha` (`fecha_hora`),
  KEY `idx_ingreso_caja` (`id_caja`),
  KEY `idx_ingreso_estado` (`estado`),
  KEY `idx_ingreso_socio` (`id_socio`),
  KEY `idx_ingreso_medio_pago` (`id_medio_pago`),
  KEY `idx_ingreso_categoria` (`id_categoria_ingreso`),
  KEY `idx_ingreso_registrado_por` (`registrado_por`),
  KEY `idx_ingreso_anulado_por` (`anulado_por`),
  CONSTRAINT `fk_ingreso_socio` FOREIGN KEY (`id_socio`) REFERENCES `socio` (`id_socio`),
  CONSTRAINT `fk_ingreso_medio_pago` FOREIGN KEY (`id_medio_pago`) REFERENCES `medio_pago` (`id_medio_pago`),
  CONSTRAINT `fk_ingreso_categoria` FOREIGN KEY (`id_categoria_ingreso`) REFERENCES `categoria_ingreso` (`id_categoria_ingreso`),
  CONSTRAINT `fk_ingreso_caja` FOREIGN KEY (`id_caja`) REFERENCES `caja` (`id_caja`),
  CONSTRAINT `fk_ingreso_registrado_por` FOREIGN KEY (`registrado_por`) REFERENCES `usuario` (`id_usuario`),
  CONSTRAINT `fk_ingreso_anulado_por` FOREIGN KEY (`anulado_por`) REFERENCES `usuario` (`id_usuario`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Nuevo: id_grupo_egreso (Estudiantes, Administración u Otros), obligatorio.
CREATE TABLE `egreso` (
  `id_egreso` int(11) NOT NULL AUTO_INCREMENT,
  `fecha_hora` datetime NOT NULL,
  `importe` decimal(10,2) NOT NULL,
  `concepto` varchar(255) NOT NULL,
  `nro_comprobante` varchar(255) DEFAULT NULL,
  `archivo_comprobante` varchar(255) DEFAULT NULL,
  `observacion` varchar(255) DEFAULT NULL,
  `estado` enum('registrado','anulado') NOT NULL DEFAULT 'registrado',
  `motivo_anulacion` varchar(200) DEFAULT NULL,
  `fecha_anulacion` datetime DEFAULT NULL,
  `id_medio_pago` int(11) NOT NULL,
  `id_categoria_egreso` int(11) NOT NULL,
  `id_grupo_egreso` int(11) NOT NULL,
  `id_caja` int(11) NOT NULL,
  `registrado_por` int(11) NOT NULL,
  `anulado_por` int(11) DEFAULT NULL,
  PRIMARY KEY (`id_egreso`),
  KEY `idx_egreso_fecha` (`fecha_hora`),
  KEY `idx_egreso_caja` (`id_caja`),
  KEY `idx_egreso_estado` (`estado`),
  KEY `idx_egreso_medio_pago` (`id_medio_pago`),
  KEY `idx_egreso_categoria` (`id_categoria_egreso`),
  KEY `idx_egreso_grupo` (`id_grupo_egreso`),
  KEY `idx_egreso_registrado_por` (`registrado_por`),
  KEY `idx_egreso_anulado_por` (`anulado_por`),
  CONSTRAINT `fk_egreso_medio_pago` FOREIGN KEY (`id_medio_pago`) REFERENCES `medio_pago` (`id_medio_pago`),
  CONSTRAINT `fk_egreso_categoria` FOREIGN KEY (`id_categoria_egreso`) REFERENCES `categoria_egreso` (`id_categoria_egreso`),
  CONSTRAINT `fk_egreso_grupo` FOREIGN KEY (`id_grupo_egreso`) REFERENCES `grupo_egreso` (`id_grupo_egreso`),
  CONSTRAINT `fk_egreso_caja` FOREIGN KEY (`id_caja`) REFERENCES `caja` (`id_caja`),
  CONSTRAINT `fk_egreso_registrado_por` FOREIGN KEY (`registrado_por`) REFERENCES `usuario` (`id_usuario`),
  CONSTRAINT `fk_egreso_anulado_por` FOREIGN KEY (`anulado_por`) REFERENCES `usuario` (`id_usuario`)
) ENGINE=InnoDB AUTO_INCREMENT=1000001 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Une un ingreso de cuota social con cada cuota que paga (un ingreso puede pagar varias).
-- pagada_por: el responsable del socio, si pagó él; NULL si pagó el propio socio.
CREATE TABLE `detalle_ingreso_cuota` (
  `id_detalle_ingreso_cuota` int(11) NOT NULL AUTO_INCREMENT,
  `id_ingreso` int(11) NOT NULL,
  `id_cuota` int(11) NOT NULL,
  `monto_aplicado` decimal(10,2) NOT NULL,
  `pagada_por` int(11) DEFAULT NULL,
  PRIMARY KEY (`id_detalle_ingreso_cuota`),
  UNIQUE KEY `uq_detalle_ingreso_cuota` (`id_ingreso`, `id_cuota`),
  KEY `idx_detalle_cuota` (`id_cuota`),
  KEY `idx_detalle_pagada_por` (`pagada_por`),
  CONSTRAINT `fk_detalle_ingreso` FOREIGN KEY (`id_ingreso`) REFERENCES `ingreso` (`id_ingreso`),
  CONSTRAINT `fk_detalle_cuota` FOREIGN KEY (`id_cuota`) REFERENCES `cuota` (`id_cuota`),
  CONSTRAINT `fk_detalle_pagada_por` FOREIGN KEY (`pagada_por`) REFERENCES `socio` (`id_socio`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- ---------------------------------------------------------------------
-- Auditoría: quién hizo qué sobre qué dato. No se modifica.
-- ---------------------------------------------------------------------

-- detalle: qué cambió, por ejemplo "Roles: Operador → Operador, Contador" (se recorta a 500).
CREATE TABLE `auditoria` (
  `id_auditoria` int(11) NOT NULL AUTO_INCREMENT,
  `id_usuario` int(11) NOT NULL,
  `entidad` varchar(50) NOT NULL,
  `id_registro` int(11) NOT NULL,
  `accion` enum('alta','baja','modificacion') NOT NULL,
  `detalle` varchar(500) DEFAULT NULL,
  `fecha_hora` datetime NOT NULL,
  `datos_previos` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id_auditoria`),
  KEY `idx_auditoria_entidad_registro` (`entidad`, `id_registro`),
  KEY `idx_auditoria_fecha` (`fecha_hora`),
  KEY `idx_auditoria_usuario` (`id_usuario`),
  CONSTRAINT `fk_auditoria_usuario` FOREIGN KEY (`id_usuario`) REFERENCES `usuario` (`id_usuario`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- =====================================================================
-- Datos iniciales: los mismos que carga Datos/DatosIniciales.cs
-- =====================================================================

INSERT INTO `rol` (`nombre`, `descripcion`) VALUES
('administrador', 'Administra usuarios, catálogos y la auditoría'),
('operador',      'Registra socios, cuotas e ingresos'),
('contador',      'Abre y cierra la caja y registra egresos');

-- Contraseñas: admin123 / operador123 / contador123 (cámbienlas al usarla de verdad).
INSERT INTO `usuario` (`nombre_usuario`, `nombre`, `apellido`, `password_hash`, `activo`, `creado_en`) VALUES
('admin',    'Ana',   'Admin',     '100000.xPMvEygnVy4B/tXRcF59MQ==.Kq0+k2Hfz838gmASM31q49tURzSIUBMnw0dXwcWI9y4=',    1, NOW()),
('operador', 'Omar',  'Operador',  '100000./T1glkucZDQ+OhxzRI2aYw==.laeKl2J58ObgrGab9g/8d+QaxKuiRbA8eUA8r5q8rO4=', 1, NOW()),
('contador', 'Carla', 'Contadora', '100000.MoAmfzDiJzsQU4EScysmAg==.hKtjsRHE1hk50J1jcaOCoTOTKxrlsNikeFxA+TE80dU=', 1, NOW());

INSERT INTO `usuario_rol` (`id_usuario`, `id_rol`)
SELECT u.`id_usuario`, r.`id_rol`
FROM `usuario` u
JOIN `rol` r ON (u.`nombre_usuario` = 'admin' AND r.`nombre` = 'administrador')
             OR (u.`nombre_usuario` = 'operador' AND r.`nombre` = 'operador')
             OR (u.`nombre_usuario` = 'contador' AND r.`nombre` = 'contador');

SET @admin := (SELECT `id_usuario` FROM `usuario` WHERE `nombre_usuario` = 'admin');

INSERT INTO `medio_pago` (`nombre`) VALUES
('Efectivo'), ('Transferencia'), ('Tarjeta'), ('Cheque');

-- Los nombres son los del enum CategoriaIngreso.
INSERT INTO `categoria_ingreso` (`nombre`, `descripcion`, `creada_por`) VALUES
('CuotaSocial', 'Cuota social', @admin),
('Donacion',    'Donación',     @admin),
('Subsidio',    'Subsidio',     @admin);

INSERT INTO `categoria_egreso` (`nombre`, `creada_por`) VALUES
('Materiales', @admin), ('Servicios', @admin), ('Mantenimiento', @admin), ('Eventos', @admin), ('Otro', @admin);

INSERT INTO `grupo_egreso` (`nombre`, `creado_por`) VALUES
('Estudiantes', @admin), ('Administración', @admin), ('Otros', @admin);

COMMIT;
