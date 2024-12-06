import 'dart:math';

import 'package:auto_route/auto_route.dart';
import 'package:flutter/material.dart';
import 'package:smartstay_app/app/router/app_router.gr.dart';

@RoutePage()
class SignupView extends StatefulWidget {
  const SignupView({super.key});

  @override
  State<SignupView> createState() => _SignupViewState();
}

class _SignupViewState extends State<SignupView> {
  bool isHovered = false;
  bool isPasswordVisible = false;

  @override
  Widget build(BuildContext context) {
    final screenWidth = MediaQuery.of(context).size.width;

    return Scaffold(
      body: Row(
        children: [
          // Left side with image
          if (screenWidth > 800)
            Expanded(
              child: Container(
                decoration: const BoxDecoration(
                  image: DecorationImage(
                    image: AssetImage('assets/images/login_page_image.png'),
                    fit: BoxFit.cover,
                  ),
                ),
              ),
            ),
          // Right side with form
          Expanded(
            child: Padding(
              padding: const EdgeInsets.symmetric(horizontal: 40),
              child: Column(
                mainAxisAlignment: MainAxisAlignment.center,
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  // Title
                  const Text(
                    'Sign Up',
                    style: TextStyle(
                      fontSize: 32,
                      fontWeight: FontWeight.bold,
                    ),
                  ),
                  const SizedBox(height: 8),
                  // Subtitle
                  const Text(
                    'Sign up to your SmartStay account and book your next getaway',
                    style: TextStyle(
                      fontSize: 16,
                      color: Colors.grey,
                    ),
                  ),
                  const SizedBox(height: 32),
                  // Form fields
                  const TextField(
                    decoration: InputDecoration(
                      labelText: "First Name",
                      border: OutlineInputBorder(
                        borderRadius: BorderRadius.all(
                          Radius.circular(12),
                        ),
                      ),
                    ),
                  ),
                  const SizedBox(height: 16),
                  const TextField(
                    decoration: InputDecoration(
                      labelText: "Last Name",
                      border: OutlineInputBorder(
                        borderRadius: BorderRadius.all(
                          Radius.circular(12),
                        ),
                      ),
                    ),
                  ),
                  const SizedBox(height: 16),
                  const TextField(
                    decoration: InputDecoration(
                      labelText: "Email address",
                      border: OutlineInputBorder(
                        borderRadius: BorderRadius.all(
                          Radius.circular(12),
                        ),
                      ),
                    ),
                  ),
                  const SizedBox(height: 16),
                  TextField(
                    obscureText: !isPasswordVisible,
                    decoration: InputDecoration(
                      labelText: 'Password',
                      border: const OutlineInputBorder(
                        borderRadius: BorderRadius.all(
                          Radius.circular(12),
                        ),
                      ),
                      suffixIcon: IconButton(
                        icon: Icon(
                          isPasswordVisible
                              ? Icons.visibility
                              : Icons.visibility_off,
                        ),
                        onPressed: () {
                          setState(() {
                            isPasswordVisible = !isPasswordVisible;
                          });
                        },
                      ),
                    ),
                  ),
                  const SizedBox(
                      height: 8), // Space between the field and helper text
                  const Text(
                    "Use 8 or more characters with a mix of letters, numbers & symbols",
                    style: TextStyle(fontSize: 12, color: Colors.grey),
                  ),

                  const SizedBox(height: 32),
                  Row(
                    children: [
                      // First button - Log In
                      Expanded(
                        child: ElevatedButton(
                          onPressed: () {
                            // Add log-in logic here
                          },
                          style: ElevatedButton.styleFrom(
                            backgroundColor: Theme.of(context)
                                .buttonTheme
                                .colorScheme
                                ?.primary,
                            foregroundColor: Colors.white,
                            padding: const EdgeInsets.symmetric(vertical: 16),
                            shape: RoundedRectangleBorder(
                              borderRadius: BorderRadius.circular(32),
                            ),
                          ),
                          child: const Text(
                            'Log In as Client',
                            style: TextStyle(fontSize: 16),
                          ),
                        ),
                      ),

                      const SizedBox(width: 20), // Space between buttons

                      // Second button - Log In (or other action)
                      Expanded(
                        child: ElevatedButton(
                          onPressed: () {
                            // Add second log-in or other logic here
                          },
                          style: ElevatedButton.styleFrom(
                            backgroundColor: Theme.of(context)
                                .buttonTheme
                                .colorScheme
                                ?.secondary,
                            foregroundColor: Colors.white,
                            padding: const EdgeInsets.symmetric(vertical: 16),
                            shape: RoundedRectangleBorder(
                              borderRadius: BorderRadius.circular(32),
                            ),
                          ),
                          child: const Text(
                            'Log In as Owner',
                            style: TextStyle(fontSize: 16),
                          ),
                        ),
                      ),
                    ],
                  ),
                  const SizedBox(height: 16),
                  Row(
                    children: [
                      const Text(
                        'Already have an account? ',
                        style: TextStyle(color: Colors.grey),
                      ),
                      MouseRegion(
                        cursor: SystemMouseCursors
                            .click, // Changes the cursor to a pointer
                        onEnter: (_) {
                          setState(() {
                            isHovered = true;
                          });
                        },
                        onExit: (_) {
                          setState(() {
                            isHovered = false;
                          });
                        },
                        child: GestureDetector(
                          onTap: () {
                            context.router.replace(const LoginRoute());
                          },
                          child: Text.rich(
                            TextSpan(
                              text: 'Log In',
                              style: TextStyle(
                                color: Colors.blue,
                                fontWeight: FontWeight.bold,
                                decoration: isHovered
                                    ? TextDecoration.underline
                                    : TextDecoration.none,
                                decorationColor: isHovered
                                    ? Colors.blue
                                    : Colors.transparent, // Underline color
                              ),
                            ),
                          ),
                        ),
                      ),
                    ],
                  ),
                ],
              ),
            ),
          ),
        ],
      ),
    );
  }
}
