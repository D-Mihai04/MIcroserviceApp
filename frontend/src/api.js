const API_BASE = import.meta.env.VITE_API_URL || "http://localhost/api";

function getAuthHeaders() {
  const token = localStorage.getItem("token");
  return {
    "Content-Type": "application/json",
    ...(token ? { Authorization: `Bearer ${token}` } : {})
  };
}

async function handleResponse(res) {
  if (res.status === 401) {
    localStorage.clear();
    window.location.reload();
    throw new Error("Session expired. Please log in again.");
  }
  if (!res.ok) {
    const errorText = await res.text();
    try {
      const parsed = JSON.parse(errorText);
      throw new Error(parsed.error || parsed.detail || res.statusText);
    } catch {
      throw new Error(errorText || res.statusText);
    }
  }
  const text = await res.text();
  return text ? JSON.parse(text) : null;
}

export const api = {
  async login(username, password) {
    const res = await fetch(`${API_BASE}/auth/login`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ username, password })
    });
    return handleResponse(res);
  },

  async register(username, password, role = 2) {
    const res = await fetch(`${API_BASE}/auth/register`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ username, password, role: Number(role) })
    });
    return handleResponse(res);
  },

  // Users (Admin only)
  async getUsers() {
    const res = await fetch(`${API_BASE}/users`, {
      headers: getAuthHeaders()
    });
    return handleResponse(res);
  },

  async createUser(userData) {
    const res = await fetch(`${API_BASE}/users`, {
      method: "POST",
      headers: getAuthHeaders(),
      body: JSON.stringify(userData)
    });
    return handleResponse(res);
  },

  async updateUser(id, userData) {
    const res = await fetch(`${API_BASE}/users/${id}`, {
      method: "PATCH",
      headers: getAuthHeaders(),
      body: JSON.stringify(userData)
    });
    return handleResponse(res);
  },

  async deleteUser(id) {
    const res = await fetch(`${API_BASE}/users/${id}`, {
      method: "DELETE",
      headers: getAuthHeaders()
    });
    return handleResponse(res);
  },

  // Devices (Admin & Client)
  async getDevices() {
    const res = await fetch(`${API_BASE}/devices`, {
      headers: getAuthHeaders()
    });
    return handleResponse(res);
  },

  async getDevicesByUserId(userId) {
    const res = await fetch(`${API_BASE}/devices/user/${userId}`, {
      headers: getAuthHeaders()
    });
    return handleResponse(res);
  },

  async createDevice(deviceData) {
    const res = await fetch(`${API_BASE}/devices`, {
      method: "POST",
      headers: getAuthHeaders(),
      body: JSON.stringify(deviceData)
    });
    return handleResponse(res);
  },

  async updateDevice(id, deviceData) {
    const res = await fetch(`${API_BASE}/devices/${id}`, {
      method: "PATCH",
      headers: getAuthHeaders(),
      body: JSON.stringify(deviceData)
    });
    return handleResponse(res);
  },

  async assignDevice(deviceId, userId) {
    const res = await fetch(`${API_BASE}/devices/${deviceId}/assign/${userId}`, {
      method: "PATCH",
      headers: getAuthHeaders()
    });
    return handleResponse(res);
  },

  async unassignDevice(deviceId) {
    const res = await fetch(`${API_BASE}/devices/${deviceId}/unassign`, {
      method: "PATCH",
      headers: getAuthHeaders()
    });
    return handleResponse(res);
  },

  async deleteDevice(id) {
    const res = await fetch(`${API_BASE}/devices/${id}`, {
      method: "DELETE",
      headers: getAuthHeaders()
    });
    return handleResponse(res);
  }
};
