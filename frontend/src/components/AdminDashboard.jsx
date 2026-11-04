import { useState, useEffect } from "react";
import { api } from "../api";

export default function AdminDashboard() {
  const [tab, setTab] = useState("users");
  const [users, setUsers] = useState([]);
  const [devices, setDevices] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");

  // New User Form State
  const [newUser, setNewUser] = useState({ name: "", email: "", password: "", userType: 2 });

  // New Device Form State
  const [newDevice, setNewDevice] = useState({ deviceName: "", consumptionValue: "", userId: "" });

  // Edit states
  const [editingUser, setEditingUser] = useState(null);
  const [editingDevice, setEditingDevice] = useState(null);

  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {
    setLoading(true);
    setError("");
    try {
      const [usersData, devicesData] = await Promise.all([
        api.getUsers(),
        api.getDevices()
      ]);
      setUsers(usersData || []);
      setDevices(devicesData || []);
    } catch (err) {
      setError(err.message || "Failed to load data.");
    } finally {
      setLoading(false);
    }
  };

  const showSuccess = (msg) => {
    setSuccess(msg);
    setTimeout(() => setSuccess(""), 3500);
  };

  // --- USER CRUD ---
  const handleCreateUser = async (e) => {
    e.preventDefault();
    setError("");
    try {
      await api.createUser({
        name: newUser.name,
        email: newUser.email,
        password: newUser.password,
        userType: Number(newUser.userType)
      });
      showSuccess("User created successfully!");
      setNewUser({ name: "", email: "", password: "", userType: 2 });
      loadData();
    } catch (err) {
      setError(err.message);
    }
  };

  const handleUpdateUser = async (e) => {
    e.preventDefault();
    if (!editingUser) return;
    setError("");
    try {
      await api.updateUser(editingUser.id, {
        name: editingUser.name,
        userType: Number(editingUser.userType)
      });
      showSuccess("User updated successfully!");
      setEditingUser(null);
      loadData();
    } catch (err) {
      setError(err.message);
    }
  };

  const handleDeleteUser = async (id) => {
    if (!confirm("Are you sure you want to delete this user? Their devices will be unassigned.")) return;
    setError("");
    try {
      await api.deleteUser(id);
      showSuccess("User deleted successfully!");
      loadData();
    } catch (err) {
      setError(err.message);
    }
  };

  const handleCreateDevice = async (e) => {
    e.preventDefault();
    setError("");
    try {
      await api.createDevice({
        deviceName: newDevice.deviceName,
        consumptionValue: Number(newDevice.consumptionValue),
        userId: newDevice.userId ? newDevice.userId : null
      });
      showSuccess("Device created successfully!");
      setNewDevice({ deviceName: "", consumptionValue: "", userId: "" });
      loadData();
    } catch (err) {
      setError(err.message);
    }
  };

  const handleUpdateDevice = async (e) => {
    e.preventDefault();
    if (!editingDevice) return;
    setError("");
    try {
      await api.updateDevice(editingDevice.id, {
        deviceName: editingDevice.deviceName,
        consumptionValue: Number(editingDevice.consumptionValue)
      });
      showSuccess("Device updated successfully!");
      setEditingDevice(null);
      loadData();
    } catch (err) {
      setError(err.message);
    }
  };

  const handleAssignDevice = async (deviceId, userId) => {
    setError("");
    try {
      if (userId) {
        await api.assignDevice(deviceId, userId);
        showSuccess("Device assigned successfully!");
      } else {
        await api.unassignDevice(deviceId);
        showSuccess("Device unassigned!");
      }
      loadData();
    } catch (err) {
      setError(err.message);
    }
  };

  const handleDeleteDevice = async (id) => {
    if (!confirm("Are you sure you want to delete this device?")) return;
    setError("");
    try {
      await api.deleteDevice(id);
      showSuccess("Device deleted successfully!");
      loadData();
    } catch (err) {
      setError(err.message);
    }
  };

  return (
    <div className="container">
      <div className="dashboard-header">
        <h2>Administrator Management</h2>
        <div className="tab-buttons">
          <button
            className={`tab-btn ${tab === "users" ? "active" : ""}`}
            onClick={() => setTab("users")}
          >
            Users ({users.length})
          </button>
          <button
            className={`tab-btn ${tab === "devices" ? "active" : ""}`}
            onClick={() => setTab("devices")}
          >
            Devices ({devices.length})
          </button>
        </div>
      </div>

      {error && <div className="alert alert-error">{error}</div>}
      {success && <div className="alert alert-success">{success}</div>}

      {/* USERS TAB */}
      {tab === "users" && (
        <div className="grid">
          <div className="card form-card">
            <h3>Add New User</h3>
            <form onSubmit={handleCreateUser}>
              <div className="form-group">
                <label>Name</label>
                <input
                  required
                  value={newUser.name}
                  onChange={(e) => setNewUser({ ...newUser, name: e.target.value })}

                />
              </div>
              <div className="form-group">
                <label>Email</label>
                <input
                  type="email"
                  required
                  value={newUser.email}
                  onChange={(e) => setNewUser({ ...newUser, email: e.target.value })}

                />
              </div>
              <div className="form-group">
                <label>Password</label>
                <input
                  type="password"
                  required
                  value={newUser.password}
                  onChange={(e) => setNewUser({ ...newUser, password: e.target.value })}
                />
              </div>
              <div className="form-group">
                <label>Role</label>
                <select
                  value={newUser.userType}
                  onChange={(e) => setNewUser({ ...newUser, userType: e.target.value })}
                >
                  <option value={2}>Client</option>
                  <option value={1}>Admin</option>
                </select>
              </div>
              <button type="submit" className="btn btn-primary btn-block">
                Create User
              </button>
            </form>
          </div>

          {/* Users Table */}
          <div className="card table-card">
            <h3>Registered Users</h3>
            {loading ? (
              <p>Loading users...</p>
            ) : users.length === 0 ? (
              <p>No users registered yet.</p>
            ) : (
              <table className="table">
                <thead>
                  <tr>
                    <th>Name</th>
                    <th>Email</th>
                    <th>Role</th>
                    <th>Actions</th>
                  </tr>
                </thead>
                <tbody>
                  {users.map((u) => (
                    <tr key={u.id}>
                      <td>{u.name}</td>
                      <td>{u.email}</td>
                      <td>
                        <span className={`badge badge-${u.userType === 1 ? "admin" : "client"}`}>
                          {u.userType === 1 ? "Admin" : "Client"}
                        </span>
                      </td>
                      <td>
                        <button
                          className="btn btn-secondary btn-sm"
                          onClick={() => setEditingUser(u)}
                        >
                          Edit
                        </button>
                        <button
                          className="btn btn-danger btn-sm"
                          onClick={() => handleDeleteUser(u.id)}
                        >
                          Delete
                        </button>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            )}
          </div>
        </div>
      )}

      {/*DEVICES TAB*/}
      {tab === "devices" && (
        <div className="grid">
          {/* Create Device Form */}
          <div className="card form-card">
            <h3>Add New Device</h3>
            <form onSubmit={handleCreateDevice}>
              <div className="form-group">
                <label>Device Name</label>
                <input
                  required
                  value={newDevice.deviceName}
                  onChange={(e) => setNewDevice({ ...newDevice, deviceName: e.target.value })}
                />
              </div>
              <div className="form-group">
                <label>Max Consumption (kWh)</label>
                <input
                  type="number"
                  required
                  min="1"
                  value={newDevice.consumptionValue}
                  onChange={(e) => setNewDevice({ ...newDevice, consumptionValue: e.target.value })}
                />
              </div>
              <div className="form-group">
                <label>Assign to User (Optional)</label>
                <select
                  value={newDevice.userId}
                  onChange={(e) => setNewDevice({ ...newDevice, userId: e.target.value })}
                >
                  <option value="">Select user</option>
                  {users.map((u) => (
                    <option key={u.id} value={u.id}>
                      {u.name} ({u.email})
                    </option>
                  ))}
                </select>
              </div>
              <button type="submit" className="btn btn-primary btn-block">
                Create Device
              </button>
            </form>
          </div>

          {/* Devices Table */}
          <div className="card table-card">
            <h3>All Devices</h3>
            {loading ? (
              <p>Loading devices...</p>
            ) : devices.length === 0 ? (
              <p>No devices registered yet.</p>
            ) : (
              <table className="table">
                <thead>
                  <tr>
                    <th>Device Name</th>
                    <th>Consumption</th>
                    <th>Assigned User</th>
                    <th>Actions</th>
                  </tr>
                </thead>
                <tbody>
                  {devices.map((d) => {
                    const assignedUser = users.find((u) => u.id === d.userId);
                    return (
                      <tr key={d.id}>
                        <td>{d.deviceName}</td>
                        <td>{d.consumptionValue} kWh</td>
                        <td>
                          <select
                            className="select-sm"
                            value={d.userId || ""}
                            onChange={(e) => handleAssignDevice(d.id, e.target.value)}
                          >
                            <option value="">-- Unassigned --</option>
                            {users.map((u) => (
                              <option key={u.id} value={u.id}>
                                {u.name}
                              </option>
                            ))}
                          </select>
                        </td>
                        <td>
                          <button
                            className="btn btn-secondary btn-sm"
                            onClick={() => setEditingDevice(d)}
                          >
                            Edit
                          </button>
                          <button
                            className="btn btn-danger btn-sm"
                            onClick={() => handleDeleteDevice(d.id)}
                          >
                            Delete
                          </button>
                        </td>
                      </tr>
                    );
                  })}
                </tbody>
              </table>
            )}
          </div>
        </div>
      )}

      {/* Edit User Modal */}
      {editingUser && (
        <div className="modal-backdrop">
          <div className="modal-card">
            <h3>Edit User</h3>
            <form onSubmit={handleUpdateUser}>
              <div className="form-group">
                <label>Name</label>
                <input
                  required
                  value={editingUser.name}
                  onChange={(e) => setEditingUser({ ...editingUser, name: e.target.value })}
                />
              </div>
              <div className="form-group">
                <label>Role</label>
                <select
                  value={editingUser.userType}
                  onChange={(e) => setEditingUser({ ...editingUser, userType: e.target.value })}
                >
                  <option value={2}>Client</option>
                  <option value={1}>Admin</option>
                </select>
              </div>
              <div className="modal-actions">
                <button type="button" className="btn btn-secondary" onClick={() => setEditingUser(null)}>
                  Cancel
                </button>
                <button type="submit" className="btn btn-primary">
                  Save Changes
                </button>
              </div>
            </form>
          </div>
        </div>
      )}

      {/* Edit Device Modal */}
      {editingDevice && (
        <div className="modal-backdrop">
          <div className="modal-card">
            <h3>Edit Device</h3>
            <form onSubmit={handleUpdateDevice}>
              <div className="form-group">
                <label>Device Name</label>
                <input
                  required
                  value={editingDevice.deviceName}
                  onChange={(e) => setEditingDevice({ ...editingDevice, deviceName: e.target.value })}
                />
              </div>
              <div className="form-group">
                <label>Max Consumption (kWh)</label>
                <input
                  type="number"
                  required
                  value={editingDevice.consumptionValue}
                  onChange={(e) => setEditingDevice({ ...editingDevice, consumptionValue: e.target.value })}
                />
              </div>
              <div className="modal-actions">
                <button type="button" className="btn btn-secondary" onClick={() => setEditingDevice(null)}>
                  Cancel
                </button>
                <button type="submit" className="btn btn-primary">
                  Save Changes
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
}
