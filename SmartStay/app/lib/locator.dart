import 'package:dio/dio.dart';
import 'package:get_it/get_it.dart';
import 'package:smartstay_app/app/environment/app_environment.dart';
import 'package:smartstay_app/core/clients/network/network_client.dart';
import 'package:smartstay_app/feature/signup/view_model/signup_viewmodel.dart';

/// [Locator] is responsible for locating and registering all the
/// services of the application.
abstract final class Locator {
  /// [GetIt] instance
  static final instance = GetIt.instance;

  /// Returns instance of [NetworkClient]
  static NetworkClient get networkClient => instance<NetworkClient>();

  /// Returns instance of [SignupViewModel]
  static SignupViewModel get signupViewModel => instance<SignupViewModel>();

  /// Responsible for registering all the dependencies
  static Future<void> locateServices(
      {required AppEnvironment environment}) async {
    instance
      // Register NetworkClient as a LazySingleton
      ..registerLazySingleton(
          () => NetworkClient(dio: instance(), baseUrl: environment.baseUrl))
      // Register SignupViewModel (factory if it has constructor parameters)
      ..registerFactory(SignupViewModel.new)
      // Register Dio as a factory
      ..registerFactory(Dio.new);
  }
}
