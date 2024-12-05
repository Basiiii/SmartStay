import 'package:auto_route/auto_route.dart';
import 'package:smartstay_app/app/router/app_router.gr.dart';

@AutoRouterConfig(replaceInRouteName: 'View,Route')

/// Holds all the routes that are defined in the app
/// Used to generate the Router object
final class AppRouter extends RootStackRouter {
  @override
  List<AutoRoute> get routes => [
        AutoRoute(
          initial: true,
          page: WelcomeRoute.page,
        ),
        AutoRoute(page: LoginRoute.page),
        AutoRoute(page: HomeRoute.page),
      ];
}
