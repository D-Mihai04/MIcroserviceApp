import { useState, useEffect } from "react";
import Login from "./components/Login";
import Navbar from "./components/Navbar";
import AdminDashboard from "./components/AdminDashboard";
import ClientDashboard from "./components/ClientDashboard";
import "./App.css";

export default function App() {
  const [user, setUser] = useState(null);
  const [checkingAuth, setCheckingAuth] = useState(true);

  useEffect(() => {
    const token = localStorage.getItem("token");
    const userId = localStorage.getItem("userId");
    const username = localStorage.getItem("username");
    const role = localStorage.getItem("role");

    if (token && role && userId) {
      setUser({ token, userId, username, role });
    }
    setCheckingAuth(false);
  }, []);

  const handleLoginSuccess = (userData) => {
    setUser(userData);
  };

  const handleLogout = () => {
    localStorage.clear();
    setUser(null);
  };

  if (checkingAuth) {
    return <div className="loading-screen">Loading application...</div>;
  }

  if (!user) {
    return <Login onLoginSuccess={handleLoginSuccess} />;
  }

  return (
    <div className="app-layout">
      <Navbar user={user} onLogout={handleLogout} />
      <main className="main-content">
        {user.role === "Admin" ? (
          <AdminDashboard />
        ) : (
          <ClientDashboard user={user} />
        )}
      </main>
    </div>
  );
}
