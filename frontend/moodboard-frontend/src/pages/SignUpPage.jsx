import React, { useState } from "react";
import { Link } from "react-router-dom";

const SignUpPage = () => {
  const [fullName, setFullName] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [agree, setAgree] = useState(false);
  const [showPassword, setShowPassword] = useState(false);
  const [errors, setErrors] = useState({});

  const validate = () => {
    const newErrors = {};
    if (!fullName.trim()) newErrors.fullName = "Full name is required";
    if (!email.match(/^[^\s@]+@[^\s@]+\.[^\s@]+$/))
      newErrors.email = "Valid email is required";
    if (password.length < 6)
      newErrors.password = "Password must be at least 6 characters";
    if (!agree) newErrors.agree = "You must accept the terms";
    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    if (validate()) {
      alert("Form submitted successfully!");      
    }
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-gradient-to-b from-gray-900 to-black text-white">
      <div className="w-80 space-y-6">
        <h2 className="text-center text-xl font-semibold">Sign Up</h2>

        <form className="space-y-4" onSubmit={handleSubmit}>
          <div>
            <label className="text-sm">Full Name</label>
            <input
              type="text"
              value={fullName}
              onChange={(e) => setFullName(e.target.value)}
              placeholder="Enter your full name"
              className="w-full mt-1 p-3 rounded-md bg-gray-800 focus:outline-none"
            />
            {errors.fullName && (
              <p className="text-red-400 text-xs mt-1">{errors.fullName}</p>
            )}
          </div>

          <div>
            <label className="text-sm">Email</label>
            <input
              type="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              placeholder="Email address"
              className="w-full mt-1 p-3 rounded-md bg-gray-800 focus:outline-none"
            />
            {errors.email && (
              <p className="text-red-400 text-xs mt-1">{errors.email}</p>
            )}
          </div>

          <div>
            <label className="text-sm">Password</label>
            <div className="relative">
              <input
                type={showPassword ? "text" : "password"}
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                placeholder="Password"
                className="w-full mt-1 p-3 rounded-md bg-gray-800 focus:outline-none"
              />
              <button
                type="button"
                onClick={() => setShowPassword(!showPassword)}
                className="absolute right-3 top-3 text-sm text-indigo-400"
              >
                {showPassword ? "Hide" : "Show"}
              </button>
            </div>
            {errors.password && (
              <p className="text-red-400 text-xs mt-1">{errors.password}</p>
            )}
          </div>

          <div className="flex items-center space-x-2">
            <input
              type="checkbox"
              checked={agree}
              onChange={() => setAgree(!agree)}
              className="accent-indigo-600"
            />
            <p className="text-sm">
              I agree to the{" "}
              <span className="text-indigo-400">Terms of Service</span> and{" "}
              <span className="text-indigo-400">Privacy Policy</span>.
            </p>
          </div>
          {errors.agree && (
            <p className="text-red-400 text-xs mt-1">{errors.agree}</p>
          )}

          <button
            type="submit"
            disabled={!agree}
            className={`w-full py-3 rounded-md font-medium ${
              agree
                ? "bg-indigo-600 hover:bg-indigo-700"
                : "bg-gray-700 cursor-not-allowed"
            }`}
          >
            Sign Up
          </button>

          <div className="space-y-3">
            <button
              type="button"
              className="w-full bg-gray-800 py-3 rounded-md"
            >
              Sign up with Google
            </button>
            <button
              type="button"
              className="w-full bg-gray-800 py-3 rounded-md"
            >
              Continue with Apple
            </button>
          </div>

          <p className="text-center text-sm text-gray-400">
            Already have an account?{" "}
            <Link to="/login" className="text-indigo-400">
              Log in
            </Link>
          </p>
        </form>
      </div>
    </div>
  );
};

export default SignUpPage;

