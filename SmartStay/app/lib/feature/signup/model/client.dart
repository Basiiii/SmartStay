class Client {
  Client({
    required this.id,
    required this.firstName,
    required this.lastName,
    required this.email,
  });

  factory Client.fromJson(Map<String, dynamic> json) {
    return Client(
      id: (json['id'] ?? '').toString(),
      firstName: (json['firstName'] ?? '').toString(),
      lastName: (json['lastName'] ?? '').toString(),
      email: (json['email'] ?? '').toString(),
    );
  }

  final String id;
  final String firstName;
  final String lastName;
  final String email;
}
