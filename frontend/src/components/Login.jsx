import { useState } from "react";
import { api } from "../api";

export default function Login({ onLoginSuccess }) {
  const [isRegister, setIsRegister] = useState(false);
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [role, setRole] = useState(2);
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError("");
    setLoading(true);

    try {
      let data;
      if (isRegister) {
        data = await api.register(username, password, role);
      } else {
        data = await api.login(username, password);
      }

      localStorage.setItem("token", data.token);
      localStorage.setItem("userId", data.userId);
      localStorage.setItem("username", data.username);
      localStorage.setItem("role", data.role);
      onLoginSuccess(data);
    } catch (err) {
      setError(err.message || (isRegister ? "Registration failed." : "Failed to log in."));
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="login-container">
      <div className="login-card">
        <h2>Energy Management System</h2>
        <p className="subtitle">
          {isRegister ? "Create a new account" : "Sign in to your account"}
        </p>

        {/* <div className="tab-buttons" style={{ marginBottom: "1.5rem", justifyContent: "center" }}>
          <button
            type="button"
            className={`tab-btn ${!isRegister ? "active" : ""}`}
            onClick={() => { setIsRegister(false); setError(""); }}
          >
            Sign In
          </button>
          <button
            type="button"
            className={`tab-btn ${isRegister ? "active" : ""}`}
            onClick={() => { setIsRegister(true); setError(""); }}
          >
            Register
          </button>
        </div> */}

        {error && <div className="alert alert-error">{error}</div>}

        <form onSubmit={handleSubmit}>
          <div className="form-group">
            <label>Username / Email</label>
            <input
              type="text"
              required
              value={username}
              onChange={(e) => setUsername(e.target.value)}
              placeholder="e.g. admin or john@example.com"
            />
          </div>

          <div className="form-group">
            <label>Password</label>
            <input
              type="password"
              required
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              placeholder="Enter your password"
            />
          </div>

          {isRegister && (
            <div className="form-group">
              <label>Account Role</label>
              <select
                value={role}
                onChange={(e) => setRole(Number(e.target.value))}
              >
                <option value={2}>Client</option>
                <option value={1}>Admin</option>
              </select>
            </div>
          )}

          <button type="submit" className="btn btn-primary btn-block" disabled={loading}>
            {loading ? "Processing..." : isRegister ? "Create Account & Sign In" : "Sign In"}
          </button>
        </form>

        <div style={{ marginTop: "1rem", textAlign: "center", fontSize: "0.85rem" }}>
          {isRegister ? (
            <span>
              Already have an account?{" "}
              <a
                href="#"
                style={{ color: "var(--primary)", textDecoration: "none", fontWeight: 500 }}
                onClick={(e) => { e.preventDefault(); setIsRegister(false); setError(""); }}
              >
                Sign In
              </a>
            </span>
          ) : (
            <span>
              Don't have an account?{" "}
              <a
                href="#"
                style={{ color: "var(--primary)", textDecoration: "none", fontWeight: 500 }}
                onClick={(e) => { e.preventDefault(); setIsRegister(true); setError(""); }}
              >
                Register
              </a>
            </span>
          )}
        </div>
      </div>
    </div>
  );
}
