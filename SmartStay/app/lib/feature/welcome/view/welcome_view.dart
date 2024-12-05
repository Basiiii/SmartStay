import 'package:auto_route/auto_route.dart';
import 'package:flutter/material.dart';

@RoutePage()
class WelcomeView extends StatelessWidget {
  const WelcomeView({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: Column(
        mainAxisAlignment: MainAxisAlignment.spaceBetween,
        children: [
          // Top Image Section
          Expanded(
            child: Center(
              child: Image.asset(
                'assets/images/welcome_vector.jpg',
                fit: BoxFit.cover,
              ),
            ),
          ),

          // Title and Subtitle Section
          Padding(
            padding: const EdgeInsets.symmetric(horizontal: 24.0),
            child: Column(
              children: [
                Text(
                  'SmartStay',
                  style: Theme.of(context).textTheme.headlineLarge?.copyWith(
                        fontWeight: FontWeight.bold,
                      ),
                ),
                const SizedBox(height: 8),
                Text(
                  'Experience the ultimate hospitality for guests and owners.',
                  textAlign: TextAlign.center,
                  style: Theme.of(context).textTheme.bodyLarge,
                ),
              ],
            ),
          ),

          // Buttons Section
          Padding(
            padding:
                const EdgeInsets.symmetric(horizontal: 24.0, vertical: 32.0),
            child: Column(
              children: [
                ElevatedButton(
                  onPressed: () {
                    // Navigate to Client Login
                    // context.router.push(ClientLoginRoute());
                  },
                  style: ElevatedButton.styleFrom(
                    minimumSize: const Size.fromHeight(50),
                  ),
                  child: const Text('Client Login'),
                ),
                const SizedBox(height: 16),
                ElevatedButton(
                  onPressed: () {
                    // Navigate to Accommodation Owner Login
                    // context.router.push(OwnerLoginRoute());
                  },
                  style: ElevatedButton.styleFrom(
                    minimumSize: const Size.fromHeight(50),
                    backgroundColor: Colors.grey, // Optional: A different color
                  ),
                  child: const Text('Owner Login'),
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }
}
