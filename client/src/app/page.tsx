'use client';

import { useRouter } from 'next/navigation';
import SessionCard from './components/SessionCard';

export default function Home() {
  const router = useRouter();

  const handleSessionSelect = (type: 'personal' | 'enterprise') => {
    // Store the session type in localStorage
    localStorage.setItem('selectedSessionType', type);
    // Redirect to login page
    router.push('/login');
  };

  const personalFeatures = [
    'Easy appointment scheduling',
    'Calendar integration',
    'Reminder notifications'
  ];

  const enterpriseFeatures = [
    'Team scheduling',
    'Resource management',
    'Advanced analytics'
  ];

  return (
    <main className="min-h-screen bg-gradient-to-b from-gray-50 to-gray-100">
      <div className="container mx-auto px-4 py-16">
        {/* Hero Section */}
        <div className="text-center mb-16">
          <h1 className="text-5xl font-bold text-gray-900 mb-4">
            Schedule Smarter, Not Harder
          </h1>
          <p className="text-xl text-gray-600 max-w-2xl mx-auto">
            Choose the perfect scheduling solution for your needs. Whether you're managing personal appointments or coordinating enterprise-wide schedules, we've got you covered.
          </p>
        </div>

        {/* Options Section */}
        <div className="grid md:grid-cols-2 gap-8 max-w-4xl mx-auto">
          <SessionCard
            type="personal"
            title="Personal"
            description="Perfect for individuals managing personal appointments, meetings, and daily schedules."
            features={personalFeatures}
            icon={
              <svg fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z" />
              </svg>
            }
            buttonText="Get Started"
            onClick={() => handleSessionSelect('personal')}
          />

          <SessionCard
            type="enterprise"
            title="Enterprise"
            description="Advanced scheduling solutions for teams and organizations of any size."
            features={enterpriseFeatures}
            icon={
              <svg fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M19 21V5a2 2 0 00-2-2H7a2 2 0 00-2 2v16m14 0h2m-2 0h-5m-9 0H3m2 0h5M9 7h1m-1 4h1m4-4h1m-1 4h1m-5 10v-5a1 1 0 011-1h2a1 1 0 011 1v5m-4 0h4" />
              </svg>
            }
            buttonText="Contact Sales"
            onClick={() => handleSessionSelect('enterprise')}
          />
        </div>
      </div>
    </main>
  );
}
