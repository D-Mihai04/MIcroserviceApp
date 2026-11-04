import { useState, useEffect } from "react";
import { api } from "../api";

export default function ClientDashboard({ user }) {
  const [devices, setDevices] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  useEffect(() => {
    if (user?.userId) {
      loadClientDevices();
    }
  }, [user]);

  const loadClientDevices = async () => {
    setLoading(true);
    setError("");
    try {
      const data = await api.getDevicesByUserId(user.userId);
      setDevices(data || []);
    } catch (err) {
      setError(err.message || "Failed to load assigned devices.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="container">
      <div className="dashboard-header">
        <h2>My Energy Metering Devices</h2>
        <button className="btn btn-secondary btn-sm" onClick={loadClientDevices} disabled={loading}>
          {loading ? "Refreshing..." : "Refresh"}
        </button>
      </div>

      {error && <div className="alert alert-error">{error}</div>}

      <div className="card table-card">
        {loading ? (
          <p>Loading your devices...</p>
        ) : devices.length === 0 ? (
          <p>No devices currently assigned to your account.</p>
        ) : (
          <table className="table">
            <thead>
              <tr>
                <th>Device Name</th>
                <th>Max Hourly Consumption</th>
                <th>Device ID</th>
              </tr>
            </thead>
            <tbody>
              {devices.map((device) => (
                <tr key={device.id}>
                  <td>
                    <strong>{device.deviceName}</strong>
                  </td>
                  <td>{device.consumptionValue} kWh</td>
                  <td>
                    <code className="code-text">{device.id}</code>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </div>
    </div>
  );
}
