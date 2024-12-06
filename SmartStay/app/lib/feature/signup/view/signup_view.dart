import 'package:auto_route/auto_route.dart';
import 'package:dio/dio.dart';
import 'package:flutter/foundation.dart';
import 'package:flutter/material.dart';
import 'package:smartstay_app/app/constants/string_constants.dart';
import 'package:smartstay_app/app/router/app_router.gr.dart';
import 'package:smartstay_app/core/clients/network/network_client.dart';

@RoutePage()
class SignupView extends StatefulWidget {
  const SignupView({super.key});

  @override
  State<SignupView> createState() => _SignupViewState();
}

class _SignupViewState extends State<SignupView> {
  bool isHovered = false;
  bool isPasswordVisible = false;

  final _firstNameController = TextEditingController();
  final _lastNameController = TextEditingController();
  final _emailController = TextEditingController();

  final NetworkClient _networkClient = NetworkClient(
    dio: Dio(),
    baseUrl: StringConstants.apiBaseURL,
  );

  Future<Map<String, dynamic>?> createBasicClient({
    required String firstName,
    required String lastName,
    required String email,
  }) async {
    try {
      // Make a POST request to the /basic endpoint
      final response = await _networkClient.post<Map<String, dynamic>>(
        '/client/basic',
        data: {
          'firstName': firstName,
          'lastName': lastName,
          'email': email,
        },
      );

      // Check if the response indicates success
      if (response.statusCode == 201) {
        // Return the newly created client data
        return response.data;
      } else {
        // Handle unexpected status codes
        _showErrorMessage('Unexpected status code: ${response.statusCode}');
        return null;
      }
    } on DioException catch (e) {
      if (e.response != null && e.response?.statusCode == 400) {
        // Handle 400 Bad Request
        final errorMessage =
            e.response?.data['message'] ?? 'Unknown error occurred';
        _showErrorMessage('Client creation failed: $errorMessage');
        return null;
      } else {
        // Handle other Dio errors (e.g., network issues)
        _showErrorMessage('Network error: ${e.message}');
        return null;
      }
    } catch (e) {
      // Handle other exceptions
      _showErrorMessage('Unexpected error: $e');
      return null;
    }
  }

  Future<bool> _handleCreateClient() async {
    // Get values from controllers
    final firstName = _firstNameController.text;
    final lastName = _lastNameController.text;
    final email = _emailController.text.trim();

    // Validate input (basic checks)
    if (firstName.isEmpty || lastName.isEmpty || email.isEmpty) {
      _showErrorMessage('Please fill in all the fields.');
      return false;
    }

    // Call the function to create the client
    final result = await createBasicClient(
      firstName: firstName,
      lastName: lastName,
      email: email,
    );

    if (result != null) {
      // Successfully created the client
      if (kDebugMode) {
        print('Client created successfully: $result');
      }
      return true;
    } else {
      // Failed to create the client, error will be shown in the _showErrorMessage
      return false;
    }
  }

  void _showErrorMessage(String message) {
    showDialog(
      context: context,
      builder: (context) => AlertDialog(
        title: const Text('Error'),
        content: Text(message),
        actions: [
          TextButton(
            onPressed: () => Navigator.of(context).pop(),
            child: const Text('OK'),
          ),
        ],
      ),
    );
  }

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
                  TextField(
                    controller: _firstNameController,
                    decoration: const InputDecoration(
                      labelText: 'First Name',
                      border: OutlineInputBorder(
                        borderRadius: BorderRadius.all(
                          Radius.circular(12),
                        ),
                      ),
                    ),
                  ),
                  const SizedBox(height: 16),
                  TextField(
                    controller: _lastNameController,
                    decoration: const InputDecoration(
                      labelText: 'Last Name',
                      border: OutlineInputBorder(
                        borderRadius: BorderRadius.all(
                          Radius.circular(12),
                        ),
                      ),
                    ),
                  ),
                  const SizedBox(height: 16),
                  TextField(
                    controller: _emailController,
                    decoration: const InputDecoration(
                      labelText: 'Email address',
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
                    height: 8,
                  ), // Space between the field and helper text
                  const Text(
                    'Use 8 or more characters with a mix of letters, numbers & symbols',
                    style: TextStyle(fontSize: 12, color: Colors.grey),
                  ),

                  const SizedBox(height: 32),
                  Row(
                    children: [
                      // First button - Sign Up as Client
                      Expanded(
                        child: ElevatedButton(
                          onPressed: () async {
                            final clientCreated = await _handleCreateClient();
                            if (clientCreated && context.mounted) {
                              await context.router.replace(const HomeRoute());
                            }
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
                            'Sign Up as Client',
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
