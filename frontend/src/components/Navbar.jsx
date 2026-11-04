export default function Navbar({ user, onLogout }) {
  return (
    <nav className="navbar">
      <div className="navbar-brand">
        <strong>Energy Management System</strong>
      </div>
      <div className="navbar-user">
        <span className="username">{user.username}</span>
        <span className={`badge badge-${user.role?.toLowerCase()}`}>
          {user.role}
        </span>
        <button className="btn btn-secondary btn-sm" onClick={onLogout}>
          Sign Out
        </button>
      </div>
    </nav>
  );
}
