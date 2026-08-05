import React, { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { Eye, EyeOff } from "lucide-react";
import googleIcon from "../assets/google.png";
import appleIcon from "../assets/apple.png";
import "../styles/index.css";
import "../index.css";
import { authService } from "../services/authService";

const LoginPage = () => {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [showPassword, setShowPassword] = useState(false);
    const [errors, setErrors] = useState({});
    const [serverError, setServerError] = useState("");
    const [isLoading, setIsLoading] = useState(false);
    const navigate = useNavigate();

    const handleSubmit = async (e) => {
        e.preventDefault();
        setServerError("");
        const newErrors = {};

        if (!email) newErrors.email = "Email is required";
        else if (!/\S+@\S+\.\S+/.test(email)) newErrors.email = "Invalid email";

        if (!password) newErrors.password = "Password is required";

        setErrors(newErrors);

        if (Object.keys(newErrors).length > 0) return;
        
        setIsLoading(true);
        try {
            const result = await authService.login(email, password);
            localStorage.setItem("authToken", result.token);
            navigate("/loading");
        } catch (err) {
            setServerError("Невірний email або пароль");
        } finally {
            setIsLoading(false);
        }

    };

    return (
        <div className="min-h-screen flex items-center justify-center bg-gradient-to-b from-gray-900 to-black text-white px-4">
            <div className="w-full max-w-xs space-y-5">
                <h2 className="text-sm text-white">Log in</h2>

                <form onSubmit={handleSubmit} className="space-y-4">
                    <div>
                        <label className="text-sm text-white">Email</label>
                        <input
                            type="email"
                            placeholder="Email address"
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                            className="custom-placeholder w-full mt-1 p-3 rounded-xl bg-gray-800/80 border border-gray-700 text-sm focus:outline-none focus:ring-2 focus:ring-indigo-500"
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
                                placeholder="Password"
                                value={password}
                                onChange={(e) => setPassword(e.target.value)}
                                className="custom-placeholder w-full mt-1 p-3 rounded-xl bg-gray-800/80 border border-gray-700 text-sm focus:outline-none focus:ring-2 focus:ring-indigo-500"
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
                    {serverError && (
                        <p className="text-red-400 text-xs text-center">{serverError}</p>
                    )}
                    <p className="text-sm text-center text-gray cursor-pointer hover:text-indigo-400">
                        Forgot your password?
                    </p>

                    <button
                        type="submit"
                        disabled={isLoading}
                        className="w-full py-3 rounded-full font-medium bg-indigo-600 hover:bg-indigo-700"
                    >
                        {isLoading ? "Вхід..." : "Log in"}
                    </button>

                    <p className="text-center text-sm text-gray">
                        Not on our platform yet?{" "}
                        <span className="text-white cursor-pointer">Sign up</span>
                    </p>

                    <div className="space-y-3">
                        <button className="w-full flex items-center justify-center gap-2 bg-gray-800/80 border border-gray-700 py-3 rounded-xl text-sm hover:bg-gray-700/80 transition-colors">
                            <img src={googleIcon} alt="Google" width="16" height="16" />
                            Continue with Google
                        </button>
                        <button className="w-full flex items-center justify-center gap-2 bg-gray-800/80 border border-gray-700 py-3 rounded-xl text-sm hover:bg-gray-700/80 transition-colors">
                            <img src={appleIcon} alt="Apple" width="16" height="16" />
                            Continue with Apple
                        </button>
                    </div>
                </form>

                <p className="text-xs text-center text-gray w-[266px] mx-auto leading-snug px-6 py-4">
                    By continuing, you agree to our{" "}
                    <span className="text-white font-semibold">Terms of Service{" "}</span>
                    and acknowledge that you've read our{" "}
                    <span className="text-white font-semibold">Privacy Policy</span>.
                </p>
            </div>
        </div>
    );
};

export default LoginPage;