import React, { useState } from "react";
import { Link } from "react-router-dom";
import { Eye, EyeOff } from "lucide-react";
import googleIcon from "../assets/google.png";
import appleIcon from "../assets/apple.png";
import "../styles/index.css";

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
        <div className="min-h-screen flex items-center justify-center bg-gradient-to-b from-gray-900 to-black text-white px-4">
            <div className="w-full max-w-xs space-y-5">
                <form className="space-y-4" onSubmit={handleSubmit}>
                    <div>
                        <label className="text-sm text-white">Full Name</label>
                        <input
                            type="text"
                            value={fullName}
                            onChange={(e) => setFullName(e.target.value)}
                            placeholder="Enter your full name"
                            className="custom-placeholder w-full mt-1 p-3 rounded-xl bg-gray-800/80 border border-gray-700 text-sm focus:outline-none focus:ring-2 focus:ring-indigo-500"
                        />
                        {errors.fullName && (
                            <p className="text-red-400 text-xs mt-1">{errors.fullName}</p>
                        )}
                    </div>

                    <div>
                        <input
                            type="email"
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                            placeholder="Email address"
                            className="custom-placeholder w-full mt-1 p-3 rounded-xl bg-gray-800/80 border border-gray-700 text-sm focus:outline-none focus:ring-2 focus:ring-indigo-500"
                        />
                        {errors.email && (
                            <p className="text-red-400 text-xs mt-1">{errors.email}</p>
                        )}
                    </div>

                    <div>
                        <div className="relative">
                            <input
                                type={showPassword ? "text" : "password"}
                                value={password}
                                onChange={(e) => setPassword(e.target.value)}
                                placeholder="Password"
                                className="custom-placeholder w-full mt-1 p-3 pr-10 rounded-xl bg-gray-800/80 border border-gray-700 text-sm focus:outline-none focus:ring-2 focus:ring-indigo-500"
                            />
                            <button
                                type="button"
                                onClick={() => setShowPassword(!showPassword)}
                                className="absolute right-3 top-1/2 -translate-y-1/2 text-gray-400 hover:text-gray-200"
                            >
                                {showPassword ? <EyeOff size={18} /> : <Eye size={18} />}
                            </button>
                        </div>
                        {errors.password && (
                            <p className="text-red-400 text-xs mt-1">{errors.password}</p>
                        )}
                    </div>

                    <div className="flex items-start space-x-2 pt-1">
                        <input
                            type="checkbox"
                            checked={agree}
                            onChange={() => setAgree(!agree)}
                            className="mt-0.5 accent-indigo-600"
                        />
                        <p className="text-xs text-gray leading-snug">
                            I agree to the{" "}
                            <span className="text-white span-text">Terms of Service</span>{" "}
                            and{" "}
                            <span className="text-white span-text">Privacy Policy</span>.
                        </p>
                    </div>
                    {errors.agree && (
                        <p className="text-red-400 text-xs -mt-2">{errors.agree}</p>
                    )}

                    <button
                        type="submit"
                        disabled={!agree}
                        className={`w-full py-3 rounded-xl font-medium text-sm transition-colors ${agree
                                ? "bg-indigo-600 hover:bg-indigo-700"
                                : "bg-gray-700 cursor-not-allowed text-gray-400"
                            }`}
                    >
                        Sign Up
                    </button>

                    <div className="space-y-3 pt-1">
                        <button
                            type="button"
                            className="w-full flex items-center justify-center gap-2 bg-gray-800/80 border border-gray-700 py-3 rounded-xl text-sm hover:bg-gray-700/80 transition-colors"
                        >
                            <img src={googleIcon} alt="Google" width="16" height="16" />


                            Sign up with Google
                        </button>
                        <button
                            type="button"
                            className="w-full flex items-center justify-center gap-2 bg-gray-800/80 border border-gray-700 py-3 rounded-xl text-sm hover:bg-gray-700/80 transition-colors"
                        >
                            <img src={appleIcon} alt="Apple" width="16" height="16" />
                            Continue with Apple
                        </button>
                    </div>

                    <p className="text-center text-sm text-gray-400 pt-1">
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

