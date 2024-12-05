import 'package:auto_route/auto_route.dart';
import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:smartstay_app/app/l10n/l10n.dart';

@RoutePage()
class LoginView extends StatefulWidget {
  const LoginView({super.key});

  @override
  State<LoginView> createState() => _LoginViewState();
}

class _LoginViewState extends State<LoginView> {
  // @override
  @override
  Widget build(BuildContext context) {
    bool _obscureText = true;
    void _toggle() {
      setState(() {
        _obscureText = !_obscureText;
      });
    }

    final TextEditingController emailController = TextEditingController();
    final TextEditingController passwordController = TextEditingController();

    // email field
    final emailField = TextFormField(
      autofocus: false,
      controller: emailController,
      keyboardType: TextInputType.emailAddress,
      validator: (value) {
        if (value!.isEmpty) {
          // return AppLocalizations.of(context)!.enteremail_error;
          return "waawf";
        }
        // reg ex for email validation
        if (!RegExp("^[a-zA-Z0-9+_.-]+@[a-zA-Z0-9.-]+.[a-z]").hasMatch(value)) {
          // return AppLocalizations.of(context)!.entervalidemail_error;
          "bad email";
        }
        return null;
      },
      onSaved: (value) {
        emailController.text = value!;
      },
      textInputAction: TextInputAction.next,
      decoration: InputDecoration(
        // prefixIcon: const Icon(Icons.mail_rounded, color: Colors.white),
        prefixIcon: Padding(
          padding: const EdgeInsetsDirectional.only(start: 12.0),
          child: Icon(
            Icons.mail_rounded,
            color: Colors.white,
            size: 23,
          ),
        ),
        contentPadding: const EdgeInsets.fromLTRB(10, 0, 20, 0),
        // hintText: AppLocalizations.of(context)!.email_field,
        hintText: "awdawd",
        border: OutlineInputBorder(
          borderRadius: BorderRadius.circular(30),
        ),
        focusedBorder: OutlineInputBorder(
          borderRadius: const BorderRadius.all(Radius.circular(20.0)),
          // borderSide: BorderSide(color: colorVar, width: 1.5),
        ),
      ),
    );

    // password field
    final passwordField = TextFormField(
      autofocus: false,
      controller: passwordController,
      autocorrect: false,
      enableSuggestions: false,
      validator: (value) {
        RegExp regex = RegExp(r'^.{6,}$');
        if (value!.isEmpty) {
          // return ("Password is required for login");
          // return AppLocalizations.of(context)!.enterpass_error;
          "bad pass";
        }
        if (!regex.hasMatch(value)) {
          return ("Enter a valid password (Min. 6 characters)");
          // return AppLocalizations.of(context)!.entervalidpass_error;
        }
        return null;
      },
      onSaved: (value) {
        passwordController.text = value!;
      },
      textInputAction: TextInputAction.next,
      decoration: InputDecoration(
        prefixIcon: Padding(
          padding: const EdgeInsetsDirectional.only(start: 12.0),
          child: Icon(
            Icons.vpn_key,
            // color: darkModeOn(context) ? Colors.white : Colors.black87,
            color: Colors.white,
            size: 23,
          ),
        ),
        suffixIcon: IconButton(
          icon: Icon(
            _obscureText ? Icons.visibility_off : Icons.visibility,
          ),
          onPressed: _toggle,
        ),
        contentPadding: const EdgeInsets.fromLTRB(20, 0, 20, 0),
        // hintText: AppLocalizations.of(context)!.password_field,
        hintText: "afawfwfa",
        border: OutlineInputBorder(
          borderRadius: BorderRadius.circular(20),
        ),
        focusedBorder: OutlineInputBorder(
          borderRadius: const BorderRadius.all(Radius.circular(20.0)),
          // borderSide: BorderSide(color: colorVar, width: 1.5),
        ),
      ),
      obscureText: _obscureText,
    );

    // login button
    final loginButton = Material(
      elevation: 5,
      borderRadius: BorderRadius.circular(30),
      color: const Color.fromARGB(255, 59, 28, 255),
      child: CupertinoButton(
        padding: const EdgeInsets.fromLTRB(20, 15, 20, 15),
        // minWidth: MediaQuery.of(context).size.width,
        onPressed: () async {
          // signIn(emailController.text, passwordController.text);
        },
        child: Text(
          // AppLocalizations.of(context)!.login_button,
          "afwfwfaaw",
          textAlign: TextAlign.center,
          style: const TextStyle(fontSize: 15, color: Colors.white),
        ),
      ),
    );

    // signup button
    final signupButton = Material(
      elevation: 5,
      borderRadius: BorderRadius.circular(30),
      color: Colors.red,
      child: MaterialButton(
        padding: const EdgeInsets.fromLTRB(20, 15, 20, 15),
        // minWidth: MediaQuery.of(context).size.width,
        // shape: StadiumBorder(
        //   side: BorderSide(color: colorVar, width: 1.5),
        // ),
        onPressed: () {
          // Navigator.push(
          //     context,
          //     MaterialPageRoute(
          //         builder: (context) => const RegistrationScreen()));
        },
        child: Text(
          // AppLocalizations.of(context)!.signup_button,
          "aaaa",
          textAlign: TextAlign.center,
          // style: TextStyle(fontSize: 15, color: colorVar),
        ),
      ),
    );

    // main
    return Scaffold(
      body: Center(
        child: SingleChildScrollView(
          child: Padding(
            padding: const EdgeInsets.all(36.0),
            child: Form(
              // key: _formKey,
              child: Column(
                mainAxisAlignment: MainAxisAlignment.start,
                crossAxisAlignment: CrossAxisAlignment.start,
                children: <Widget>[
                  const SizedBox(height: 60),
                  // SizedBox(
                  //     height: 65,
                  //     child: Image.asset(
                  //       (darkModeOn(context)
                  //           ? "assets/logo_white.png"
                  //           : "assets/logo.png"),
                  //     )
                  //     // child: Image.asset(
                  //     //   "assets/logo.png",
                  //     //   fit: BoxFit.contain,
                  //     // ),
                  //     ),
                  const SizedBox(height: 20),
                  Text("title"
                      // style: Theme.of(context).textTheme.displayMedium,
                      ),
                  const SizedBox(height: 40),
                  Text("aaaa"),
                  const SizedBox(height: 8),
                  emailField,
                  const SizedBox(height: 25),
                  Text("aaaa"),
                  const SizedBox(height: 8),
                  passwordField,
                  const SizedBox(height: 20),
                  Row(
                    mainAxisAlignment: MainAxisAlignment.end,
                    children: <Widget>[
                      GestureDetector(
                        onTap: () {
                          // Navigator.push(
                          //     context,
                          //     MaterialPageRoute(
                          //         builder: (context) => const ResetPassword()));
                        },
                        child: Text(
                          // (AppLocalizations.of(context)!.resetpass_button),
                          "awfwafwa",
                          style: TextStyle(
                            // Colors.white,
                            fontSize: 15,
                            decoration: TextDecoration.underline,
                          ),
                        ),
                      ),
                    ],
                  ),
                  const SizedBox(height: 50),
                  Row(
                    mainAxisAlignment: MainAxisAlignment.center,
                    crossAxisAlignment: CrossAxisAlignment.center,
                    children: [
                      Column(
                        mainAxisAlignment: MainAxisAlignment.center,
                        crossAxisAlignment: CrossAxisAlignment.center,
                        children: <Widget>[
                          SizedBox(
                            width: 220,
                            height: 60,
                            child: loginButton,
                          ),
                          const SizedBox(height: 15),
                          SizedBox(
                            width: 220,
                            height: 60,
                            child: signupButton,
                          ),
                          const SizedBox(height: 70),
                        ],
                      ),
                    ],
                  ),
                ],
              ),
            ),
          ),
        ),
      ),
    );
  }
}
