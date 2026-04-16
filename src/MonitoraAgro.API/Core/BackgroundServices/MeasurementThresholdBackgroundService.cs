using MonitoraAgro.API.Application.UseCases.Measurements.AnalyzeThreshold;
using MonitoraAgro.API.Domain.Services.SensorMeasurement;

namespace MonitoraAgro.API.Core.BackgroundServices;

public class MeasurementThresholdBackgroundService : BackgroundService
{
    private readonly ISensorMeasurementQueue _sensorMeasurementQueue;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<MeasurementThresholdBackgroundService> _logger;

    public MeasurementThresholdBackgroundService(
        ISensorMeasurementQueue sensorMeasurementQueue,
        IServiceScopeFactory serviceScopeFactory,
        ILogger<MeasurementThresholdBackgroundService> logger)
    {
        _sensorMeasurementQueue = sensorMeasurementQueue;
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var sensorMeasurementInputs = await _sensorMeasurementQueue.Dequeue();

                using var scope = _serviceScopeFactory.CreateScope();
                var analyzeMeasurementThresholdUseCase =
                    scope.ServiceProvider.GetRequiredService<IAnalyzeMeasurementThresholdUseCase>();

                await analyzeMeasurementThresholdUseCase.Execute(sensorMeasurementInputs);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao processar a análise de limite de medição.");
                await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
            }
        }
    }
}
