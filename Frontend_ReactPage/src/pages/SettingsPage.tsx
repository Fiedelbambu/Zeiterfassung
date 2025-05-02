import AdminSettingsPage from './AdminSettingsPage';


export default function SettingsPage() {
  return (
    <div className="p-8 space-y-6">
      <h1 className="text-2xl font-bold">Einstellungen</h1>
      <p className="text-gray-600">Hier können Admins Systemeinstellungen verwalten.</p>

      <div className="mt-6">
        <AdminSettingsPage />
      </div>
    </div>
  );
}
