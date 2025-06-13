import { FC } from 'react';

interface SessionCardProps {
  type: 'personal' | 'enterprise';
  title: string;
  description: string;
  features: string[];
  icon: React.ReactNode;
  buttonText: string;
  onClick: () => void;
}

/**
 * SessionCard Component
 * A reusable card component for displaying session options (Personal/Enterprise)
 * 
 * @param {SessionCardProps} props - The component props
 * @returns {JSX.Element} A styled card component
 */
const SessionCard: FC<SessionCardProps> = ({
  type,
  title,
  description,
  features,
  icon,
  buttonText,
  onClick,
}: SessionCardProps): JSX.Element => {
  const buttonColor = type === 'personal' ? 'bg-blue-600 hover:bg-blue-700' : 'bg-purple-600 hover:bg-purple-700';
  const iconBgColor = type === 'personal' ? 'bg-blue-100' : 'bg-purple-100';
  const iconColor = type === 'personal' ? 'text-blue-600' : 'text-purple-600';

  return (
    <div 
      onClick={onClick}
      className="group cursor-pointer"
    >
      <div className="bg-white rounded-xl shadow-lg p-8 transition-all duration-300 hover:shadow-xl hover:scale-105">
        <div className="text-center">
          <div className={`w-16 h-16 ${iconBgColor} rounded-full flex items-center justify-center mx-auto mb-6`}>
            <div className={`w-8 h-8 ${iconColor}`}>
              {icon}
            </div>
          </div>
          <h2 className="text-2xl font-bold text-gray-900 mb-4">{title}</h2>
          <p className="text-gray-600 mb-6">
            {description}
          </p>
          <ul className="text-left text-gray-600 space-y-2 mb-6">
            {features.map((feature, index) => (
              <li key={index} className="flex items-center">
                <svg className="w-5 h-5 text-green-500 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M5 13l4 4L19 7" />
                </svg>
                {feature}
              </li>
            ))}
          </ul>
          <button className={`w-full ${buttonColor} text-white py-3 px-6 rounded-lg font-semibold transition-colors`}>
            {buttonText}
          </button>
        </div>
      </div>
    </div>
  );
};

export default SessionCard; 