import 'package:dio/dio.dart';
import 'package:flutter/material.dart';
import 'package:smartstay_app/app/constants/string_constants.dart';
import 'package:smartstay_app/core/clients/network/network_client.dart';
import 'package:smartstay_app/feature/signup/model/client.dart';
import 'package:smartstay_app/locator.dart';

class SignupViewModel extends ChangeNotifier {
  final NetworkClient _networkClient = Locator.networkClient;

  bool _isLoading = false;
  String? _errorMessage;

  bool get isLoading => _isLoading;
  String? get errorMessage => _errorMessage;

  /// Creates a basic client
  Future<Client?> createBasicClient({
    required String firstName,
    required String lastName,
    required String email,
  }) async {
    _setLoading(true);
    try {
      final response = await _networkClient.post<Map<String, dynamic>>(
        '/client/basic',
        data: {
          'firstName': firstName,
          'lastName': lastName,
          'email': email,
        },
      );

      if (response.statusCode == 201) {
        final client = Client.fromJson(response.data!);
        return client;
      } else {
        _setErrorMessage('Unexpected status code: ${response.statusCode}');
        return null;
      }
    } on DioException catch (e) {
      if (e.response?.statusCode == 400) {
        final errorMessage = e.response?.data['message'] ?? 'Invalid request';
        _setErrorMessage(errorMessage.toString());
      } else {
        _setErrorMessage('Network error: ${e.message}');
      }
      return null;
    } catch (e) {
      _setErrorMessage('Unexpected error: $e');
      return null;
    } finally {
      _setLoading(false);
    }
  }

  /// Helper to set loading state and notify listeners
  void _setLoading(bool value) {
    _isLoading = value;
    notifyListeners();
  }

  /// Helper to set error message and notify listeners
  void _setErrorMessage(String message) {
    _errorMessage = message;
    notifyListeners();
  }

  /// Clears error message
  void clearErrorMessage() {
    _errorMessage = null;
    notifyListeners();
  }
}
