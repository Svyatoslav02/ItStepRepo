import React, { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { Eye, EyeOff } from "lucide-react";
import googleIcon from "../assets/google.png";
import appleIcon from "../assets/apple.png";
import { authService } from "../services/authService";
import { saveSession } from "../utils/auth";

const SignUpPage = () => {
    const [fullName, setFullName] = useState("");
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [agree, setAgree] = useState(false);
    const [showPassword, setShowPassword] = useState(false);
    const [errors, setErrors] = useState({});
    const [serverError, setServerError] = useState("");
    const [isLoading, setIsLoading] = useState(false);

    const navigate = useNavigate();

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

    const handleSubmit = async (e) => {
        e.preventDefault();
        setServerError("");
        if (!validate()) return;

        setIsLoading(true);
        try {
            const result = await authService.register(fullName, email, password);
            saveSession(result);
            navigate("/home");
        } catch (err) {
            setServerError(err.message);
        } finally {
            setIsLoading(false);
        }
    };

    return (
        <div className="min-h-screen flex items-center justify-center bg-gradient-to-b from-gray-900 to-black text-white px-4">
            <div className="w-full max-w-md  backdrop-blur-md rounded-2xl p-8 shadow-xl space-y-6">
                <div className="flex justify-center items-center mb-6">
                    <div className="relative">
                        <div className="absolute inset-0 w-28 h-28 rounded-full bg-gradient-to-r from-indigo-500 via-purple-600 to-pink-500 blur-3xl opacity-70 animate-pulse"></div>
                        <img
                            src="./assets/icons/GroupLogo.png"
                            alt="Logo"
                            className="relative w-16 h-16 rounded-full border border-gray-700 shadow-[0_0_25px_rgba(99,102,241,0.8),0_0_60px_rgba(147,51,234,0.6)]"
                        />
                    </div>
                </div>

                <h2 className="text-2xl font-semibold text-center">Create your account</h2>

                <form onSubmit={handleSubmit} className="space-y-5">
                    {/* Full Name */}
                    <div>
                        <label className="text-sm text-gray-300">Full Name</label>
                        <input
                            type="text"
                            value={fullName}
                            onChange={(e) => setFullName(e.target.value)}
                            placeholder="Enter your full name"
                            className="w-full mt-2 p-4 rounded-xl bg-gray-800 border border-gray-700 text-sm focus:outline-none focus:ring-2 focus:ring-indigo-500"
                        />
                        {errors.fullName && (
                            <p className="text-red-400 text-xs mt-1">{errors.fullName}</p>
                        )}
                    </div>

                    {/* Email */}
                    <div>
                        <label className="text-sm text-gray-300">Email</label>
                        <input
                            type="email"
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                            placeholder="example@gmail.com"
                            className="w-full mt-2 p-4 rounded-xl bg-gray-800 border border-gray-700 text-sm focus:outline-none focus:ring-2 focus:ring-indigo-500"
                        />
                        {errors.email && (
                            <p className="text-red-400 text-xs mt-1">{errors.email}</p>
                        )}
                    </div>

                    {/* Password */}
                    <div>
                        <label className="text-sm text-gray-300">Password</label>
                        <div className="relative">
                            <input
                                type={showPassword ? "text" : "password"}
                                value={password}
                                onChange={(e) => setPassword(e.target.value)}
                                placeholder="********"
                                className="w-full mt-2 p-4 rounded-xl bg-gray-800 border border-gray-700 text-sm focus:outline-none focus:ring-2 focus:ring-indigo-500"
                            />
                            <button
                                type="button"
                                onClick={() => setShowPassword(!showPassword)}
                                className="absolute right-3 top-1/3 -translate-y-1/2 text-gray-400 hover:text-gray-200"
                            >
                                {showPassword ? <EyeOff size={18} /> : <Eye size={18} />}
                            </button>
                        </div>
                        {errors.password && (
                            <p className="text-red-400 text-xs mt-1">{errors.password}</p>
                        )}
                    </div>

                    {/* Terms */}
                    <div className="flex items-center gap-2 text-sm text-gray-400 my-4">
                        <input
                            type="checkbox"
                            checked={agree}
                            onChange={() => setAgree(!agree)}
                            className="accent-indigo-500"
                        />
                        <span>
                            I agree to the{" "}
                            <span className="text-white font-bold">Terms of Service</span> and{" "}
                            <span className="text-white font-bold">Privacy Policy</span>
                        </span>
                    </div>
                    {errors.agree && (
                        <p className="text-red-400 text-xs mt-1">{errors.agree}</p>
                    )}

                    {/* Server error */}
                    {serverError && (
                        <p className="text-red-400 text-xs text-center">{serverError}</p>
                    )}

                    {/* Submit */}
                    <button
                        type="submit"
                        disabled={isLoading}
                        className="w-full py-3 rounded-xl font-medium bg-indigo-600 hover:bg-indigo-700 transition-colors"
                    >
                        {isLoading ? "Loading..." : "Sign up"}
                    </button>

                    {/* Link to login */}
                    <p className="text-center text-sm text-gray-400 my-5">
                        Already have an account?{" "}
                        <Link
                            to="/login"
                            className="text-white font-bold cursor-pointer hover:underline"
                        >
                            Log in
                        </Link>
                    </p>

                    {/* Social login */}
                    <div className="space-y-3">
                        <button
                            type="button"
                            className="w-full flex items-center justify-center gap-2 bg-gray-800 border border-gray-700 py-3 rounded-xl text-sm hover:bg-gray-700 transition-colors"
                        >
                            <img src={appleIcon} alt="Apple" width="16" height="16" />
                            Continue with Apple
                        </button>
                        <button
                            type="button"
                            className="w-full flex items-center justify-center gap-2 bg-gray-800 border border-gray-700 py-3 rounded-xl text-sm hover:bg-gray-700 transition-colors"
                        >
                            <img src={googleIcon} alt="Google" width="16" height="16" />
                            Continue with Google
                        </button>
                    </div>
                </form>
            </div>
        </div>
    );
};

export default SignUpPage;