import 'package:auto_route/auto_route.dart';
import 'package:flutter/material.dart';
import 'package:smartstay_app/app/l10n/l10n.dart';

@RoutePage()
class HomeView extends StatelessWidget {
  const HomeView({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text(
          context.l10n.flutter,
        ),
      ),
      body: const Text('test'),
    );
  }
}
