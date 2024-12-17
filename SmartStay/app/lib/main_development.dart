import 'package:smartstay_app/app/environment/development_environment.dart';
import 'package:smartstay_app/app/view/app.dart';
import 'package:smartstay_app/bootstrap.dart';

Future<void> main() async {
  await bootstrap(builder: App.new, environment: DevelopmentEnvironment());
}
