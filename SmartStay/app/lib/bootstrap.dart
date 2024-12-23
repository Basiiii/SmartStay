import 'dart:async';

import 'package:flutter/material.dart';
import 'package:smartstay_app/app/environment/app_environment.dart';
import 'package:smartstay_app/core/utils/device_info/device_info_utils.dart';
import 'package:smartstay_app/core/utils/logger/logger_utils.dart';
import 'package:smartstay_app/core/utils/package_info/package_info_utils.dart';
import 'package:smartstay_app/locator.dart';

Future<void> bootstrap(
    {required FutureOr<Widget> Function() builder,
    required AppEnvironment environment}) async {
  FlutterError.onError = (details) {
    LoggerUtils.instance
        .logFatalError(details.exceptionAsString(), details.stack);
  };
  await runZonedGuarded<Future<void>>(
    () async {
      WidgetsFlutterBinding.ensureInitialized();
      // Initialize Locator and Utils
      await Future.wait([
        Locator.locateServices(environment: environment),
        PackageInfoUtils.init(),
        DeviceInfoUtils.init(),
      ]);

      runApp(await builder());
    },
    (error, stackTrace) {
      LoggerUtils.instance.logFatalError(error.toString(), stackTrace);
    },
  );
}
